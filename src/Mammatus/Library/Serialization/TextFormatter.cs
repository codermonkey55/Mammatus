using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using Mammatus.Enums;

namespace Mammatus.Library.Serialization
{
    using System;
    using System.IO;

    public class TextFormatter : IFormatter
    {
        /// <summary>
        /// Support class for conversion of data from a stream.
        /// Same converter used by BinaryFormatter
        /// </summary>
        private readonly IFormatterConverter _mConverter = new FormatterConverter();

        private StreamingContext _mContext = new StreamingContext(StreamingContextStates.All);
        private ISurrogateSelector _mSelector;

        /// <summary>
        /// Registered types, keyed by the full class name
        /// </summary>
        protected Dictionary<string, Type> MRegisteredTypes
            = new Dictionary<string, Type>();

        /// <summary>
        /// This is used to build the SerializationInfo values
        /// when reading from a stream in ValueOnly Storage mode.
        ///
        /// Registered types can have their own specific mapping
        /// of values read from a stream to a specific name.
        /// eg 1st value is called ClassName
        /// 2nd value is called Item1
        /// 3rd value is called Item2....
        /// </summary>
        protected Dictionary<string, List<string>> MRegisteredNameMaps
            = new Dictionary<string, List<string>>();

        /// <summary>
        /// Common facility is to include the names of the
        /// columns as the first line in a delimitered text file.
        /// This is only used for deserializing from a stream.
        /// MUST deserialize the first line for this to be used.
        /// </summary>
        public bool UseFirstLineAsColumnNames = false;

        /// <summary>
        /// Enables putting all items output within double quotes
        /// for a CSV file storage type.
        /// </summary>
        public bool QuoteAll = false;

        /// <summary>
        /// Used to indicate the current deserialization started
        /// at the beginning of the stream, ie stream.Position == 0.
        /// The stream must also be readable.
        /// </summary>
        private bool _mFirstLine;

        /// <summary>
        /// Column names read from the last stream that was deserialized.
        /// Only populated when the first line of the stream is
        /// deserialized and UseFirstLineAsColumnNames = true.
        /// </summary>
        private readonly List<string> _mFirstLineColumnNames = null;

        /// <summary>
        /// The most recent string read using ReadLine().
        /// </summary>
        private string _mLastLineRead = string.Empty;

        /// <summary>
        /// Read only collection of the column names read from
        /// the last stream that was deserialized.
        /// Only populated when the first line of the stream is
        /// deserialized and UseFirstLineAsColumnNames = true.
        /// </summary>
        public ReadOnlyCollection<string> FirstLineColumnNames => _mFirstLineColumnNames.AsReadOnly();

        /// <summary>
        /// Only used for StorageType of ValueOnly
        /// Used to uniquely identify each value
        /// with a name.
        /// </summary>
        private string _mNameIndexFormat = "D10";

        /// <summary>
        /// Indicates the stream text format.
        /// ValueOnly expects values to be stored in a particular order.
        /// NameValue can store items in any order.
        /// </summary>
        public TextStorageType TextStorage { get; set; } = TextStorageType.NameValue;

        /// <summary>
        /// Delimiter used to separate each item.
        /// When using StorageType.CSV, this is used as the comma
        /// </summary>
        public char NameDelimiter { get; set; } = ',';

        /// <summary>
        /// When Storage is set to NameValue, the Name and
        /// Value are separated using this delimiter.
        /// </summary>
        public char ValueDelimiter { get; set; } = '=';

        /// <summary>
        /// Escape character is used to signal the formatter
        /// to treat delimiters and the escape character as
        /// part of the name or value.
        /// Escape character is not used when using StorageType.CSV.
        /// </summary>
        public char Escape { get; set; } = '\\';

        /// <summary>
        /// The most recent string read using ReadLine().
        /// </summary>
        public string LastLineRead => _mLastLineRead;

        /// <summary>
        /// Parse a string into multiple strings using the CSV format
        /// The delimiter is configurable.
        /// Double quotes are used to escape the delimiter.
        /// Double quotes can be included in a string by using 2 double quotes.
        ///
        /// This is based on the parser at the following web reference:
        /// [ref:http://knab.ws/blog/index.php?/archives/3-CSV-file-parser-and-writer-in-C-Part-1.html]
        /// [ref:http://knab.ws/blog/index.php?/archives/10-CSV-file-parser-and-writer-in-C-Part-2.html]
        /// CSV files have a very simple structure:
        /// > Each record is one line (with exceptions)
        /// > Fields are separated with commas
        /// > Leading and trailing space-characters adjacent to comma field separators are ignored
        /// > Fields with embedded commas must be delimited with double-quote characters
        /// > Fields that contain double quote characters must be surounded by double-quotes, and the embedded double-quotes must each be represented by a pair of consecutive double quotes.
        /// > A field that contains embedded line-breaks must be surounded by double-quotes
        /// > Fields with leading or trailing spaces must be delimited with double-quote characters
        /// > Fields may always be delimited with double quotes
        /// > The first record in a CSV file may be a header record containing column (field) names
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <param name="data"></param>
        public void ParseCsvString(string s, char d, ref List<string> data)
        {
            // quoted strings allows us to include the delimiter without
            // an escape character.
            bool quoted = false;
            // white space before and after will be ignored if an item
            // contains double quotes
            bool predata = true;
            bool postdata = false;
            bool eos = (s.Length == 0);
            bool eol = (s.Length == 0);
            StringBuilder item = new StringBuilder();
            char c;
            short pos = 0;
            while (!eos && !eol)
            {
                c = s[pos];

                if ((postdata || !quoted) && c == d)
                { // delimiter means end of this item, start of new
                    data.Add(item.ToString());
                    // reset item specific variables
                    quoted = false;
                    predata = true;
                    postdata = false;
                    item = new StringBuilder();
                    // next char
                    pos++;
                    eos = (s.Length == pos);
                    continue;
                }

                if ((predata || postdata || !quoted) && (c == '\x0A' || c == '\x0D'))
                {
                    // End Of Line found - save item and stop processing line
                    eol = true;
                    if (s.Length > (pos + 1))
                    {
                        if (c == '\x0D' && s[(pos + 1)] == '\x0A')
                        {
                            pos++;
                        }
                    }
                    data.Add(item.ToString());
                    // reset item specific variables
                    quoted = false;
                    predata = true;
                    postdata = false;
                    item = new StringBuilder();
                    // next char
                    pos++;
                    eos = (s.Length == pos);
                    continue;
                }

                if (predata && c == ' ')
                { // ignore leading white space
                    // next char
                    pos++;
                    eos = (s.Length == pos);
                    continue;
                }

                if (predata && c == '"')
                { // start of quoted string
                    quoted = true;
                    predata = false;
                    // next char
                    pos++;
                    eos = (s.Length == pos);
                    continue;
                }

                if (predata)
                { // item starts without quotes
                    predata = false;
                    item.Append(c);
                    // next char
                    pos++;
                    eos = (s.Length == pos);
                    continue;
                }

                if (c == '"' && quoted)
                {
                    if (s.Length > (pos + 1))
                    {
                        if (s[(pos + 1)] == '"')
                        { // save first double quote
                            item.Append(c);
                            pos++; // move to next double quote
                        }
                        else
                        { // do not save end quote
                            postdata = true;
                        }
                        // next char
                        pos++;
                        eos = (s.Length == pos);
                        continue;
                    }
                }

                // save character
                item.Append(c);
                // next char
                pos++;
                eos = (s.Length == pos);
                continue;
            }
            if (eos)
            {
                // add the last item
                data.Add(item.ToString());
            }
        }

        /// <summary>
        /// Parse a string into multiple strings using a delimiter
        /// Allows for an escape character.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="esc"></param>
        /// <param name="d"></param>
        /// <param name="data"></param>
        public void ParseString(string s, char esc, char d, ref List<string> data)
        {
            data.Clear();
            // strings use a zero based index
            int sidx = 0; // initial index
            int idx = 0; // found delimiter
            string str = string.Empty;
            while (idx >= 0)
            {
                idx = s.IndexOf(d, sidx);
                if (idx >= 0)
                {
                    var lidx = 0; // last save index
                    if (idx == sidx)
                    {
                        // null length value
                        str += s.Substring(sidx, idx - sidx);
                        data.Add(str);
                        str = string.Empty;
                        lidx = idx + 1;
                        sidx = lidx;
                    }
                    else if (s[idx - 1] == esc)
                    {
                        // escape'd character
                        // so ignore this delimiter and try again
                        str += s.Substring(sidx, idx - sidx + 1);
                        sidx = idx + 1;
                    }
                    else
                    {
                        // save string
                        str += s.Substring(sidx, idx - sidx);
                        data.Add(str);
                        str = string.Empty;
                        // move past delimiter character
                        lidx = idx + 1;
                        sidx = lidx;
                    }
                }
                else
                {
                    // could not find character
                    // save what is left
                    str += s.Substring(sidx);
                    data.Add(str);
                    str = string.Empty;
                }
            }
        }


        /// <summary>
        /// Escape all special characters using the given escape character.
        /// </summary>
        /// <param name="str">String to be escaped</param>
        /// <param name="esc">Escape character</param>
        /// <param name="specialChars">Array of Special Characters to be escaped</param>
        /// <returns>String containing escape characters</returns>
        public string EscapeString(string str, char esc, char[] specialChars)
        {
            string ret = str;

            string r = string.Empty;
            r += esc;
            r += esc;
            string o = string.Empty;
            o += esc;
            ret = ret.Replace(o, r);

            foreach (char c in specialChars)
            {
                string repl = string.Empty;
                repl += esc;
                repl += c;
                string orig = string.Empty;
                orig += c;
                ret = ret.Replace(orig, repl);
            }
            return ret;
        }

        /// <summary>
        /// Escape all special characters using the given escape character.
        /// </summary>
        /// <param name="str">String to be escaped</param>
        /// <param name="esc">Escape character</param>
        /// <returns>String containing escape characters</returns>
        public string UnEscapeString(string str, char esc)
        {
            string ret = string.Empty;

            int sidx = 0; //start of search
            int idx = 0; // last search result
            idx = str.IndexOf(esc, sidx);
            while (idx != -1)
            {
                if (idx == sidx)
                { // start of search is an escape
                    if (str.Length > idx + 1)
                    { // save character after escape character
                        sidx++; // skip escape character
                        ret += str.Substring(sidx, 1);
                        sidx++; // start at next character
                    }
                    else
                    {
                        // no more data? but we have an escape character?
                        break;
                    }
                }
                else if (idx > sidx)
                {
                    if (str.Length >= idx + 1)
                    {
                        ret += str.Substring(sidx, idx - sidx);
                        sidx = ++idx; // skip ESC character
                        if (sidx < str.Length)
                        {
                            ret += str[sidx];
                        }
                        sidx++; // start at next character
                    }
                    else
                    {
                        // found something past the end of string
                    }
                }
                if (str.Length > sidx + 1)
                {
                    idx = str.IndexOf(esc, sidx);
                }
                else
                {
                    break;
                }
            }
            // copy remaining characters
            if (sidx < str.Length)
            {
                ret += str.Substring(sidx);
            }
            return ret;
        }

        /// <summary>
        /// Register the type of an object that is serialized
        /// A type is registered against its full classname
        /// including namespace but not the assembly.
        ///
        /// Child text formatter classes can override this
        /// method and the ObjectType(data) method to use their
        /// own registration method for types.
        /// </summary>
        /// <param name="t">System.Type returned by call typeof(T)</param>
        public virtual void RegisterType(Type t)
        {
            if (!MRegisteredTypes.ContainsKey(t.FullName))
            {
                MRegisteredTypes.Add(t.FullName, t);
            }
            else
            {
                MRegisteredTypes[t.FullName] = t;
            }
        }

        /// <summary>
        /// Register a name mapping for a given type.
        /// Name mappings are used to deserialize a stream
        /// in ValueOnly storage mode into a meaningful set
        /// of name/value pairs.
        ///
        /// Also registers the Type if not already registered.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="map"></param>
        public virtual void RegisterNameMap(Type t, List<string> map)
        {
            if (!MRegisteredTypes.ContainsKey(t.FullName))
            {
                RegisterType(t);
            }
            MRegisteredNameMaps.Add(t.FullName, map);
        }

        /// <summary>
        /// After reading a line from an input stream the stream is separated
        /// into name-value pairs using the assigned delimiters to break
        /// the line into individual items.  Default delimiters are a comma ','
        /// for separating pairs and equal '=' to separate name from value.
        /// Values will be an empty string when empty or not specified.
        /// Eg. Name1=abc,Name2=def,....
        /// </summary>
        /// <param name="nameValuePairs"></param>
        /// <returns></returns>
        public virtual Type ObjectType(Dictionary<string, string> nameValuePairs)
        {
            Type t = typeof(object); // default
            // use the classname to find the relevant type
            string classname = string.Empty;
            if (TextStorage == TextStorageType.NameValue)
            {
                if (nameValuePairs.ContainsKey("SYS_ClassFullName"))
                {
                    classname = nameValuePairs["SYS_ClassFullName"];
                }
                if (MRegisteredTypes.ContainsKey(classname))
                {
                    t = MRegisteredTypes[classname];
                }
            }
            else if (TextStorage == TextStorageType.ValueOnly
                || TextStorage == TextStorageType.Csv)
            {
                if (nameValuePairs.Count > 0)
                {
                    // first value
                    uint cnt = 0;
                    string key = cnt.ToString(_mNameIndexFormat);
                    if (nameValuePairs.ContainsKey(key))
                    {
                        classname = nameValuePairs[key];
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// Return the name mapping for a specific Type.
        /// Name mappings refer to the ValueOnly storage mode
        /// where the order in which values are read are mapped
        /// to a specific field name when populating the
        /// SerializationInfo object used for deserialization.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual List<string> ObjectNameMap(Type t)
        {
            List<string> ret = null;
            if (MRegisteredNameMaps.ContainsKey(t.FullName))
            {
                ret = MRegisteredNameMaps[t.FullName];
            }
            else
            {
                if (UseFirstLineAsColumnNames)
                { // first line must be read using this formatter
                    // all future lines are assumed to use the names
                    // from the first line.
                    ret = _mFirstLineColumnNames;
                }
            }
            return ret;
        }

        /// <summary>
        /// Parse a string into name/value pairs using the
        /// defined delimiter characters.
        ///
        /// Child text formatter classes can override this
        /// method to parse an input string using their own
        /// parsing method to populate the data dictionary
        /// with values.  The data dictionary will be used
        /// to determine which object instance to create.
        /// See ObjectType(data)
        /// </summary>
        /// <param name="s">String to parse</param>
        /// <param name="esc">Escape character for delimiters</param>
        /// <param name="nd">Name delimiter</param>
        /// <param name="vd">Name-Value delimiter</param>
        /// <param name="data">Container to fill with parsed data</param>
        public virtual void DeserializeFromString(string s, char esc, char nd, char vd, ref Dictionary<string, string> data)
        {
            data.Clear();
            List<string> pairs = new List<string>();
            if (TextStorage == TextStorageType.Csv)
            {
                ParseCsvString(s, nd, ref pairs);
            }
            else
            {
                ParseString(s, esc, nd, ref pairs);
            }
            uint cnt = 0;
            foreach (string str in pairs)
            {
                if (TextStorage == TextStorageType.NameValue)
                {
                    List<string> nv = new List<string>();
                    ParseString(str, esc, vd, ref nv);
                    if (nv.Count > 0)
                    {
                        if (nv.Count == 1)
                        { // no value specified
                            string nv0 = UnEscapeString(nv[0], esc);
                            data.Add(nv0, "");
                        }
                        else if (nv.Count >= 2)
                        { // only take the first value if multiple exist
                            string nv0 = UnEscapeString(nv[0], esc);
                            string nv1 = UnEscapeString(nv[1], esc);
                            data.Add(nv0, nv1);
                        }
                    }
                }
                else
                { // ValueOnly and CSV
                    string val = UnEscapeString(str, esc);
                    data.Add(cnt.ToString(_mNameIndexFormat), val);
                    cnt++;
                }
            }
        }

        /// <summary>
        /// Read a single line of text from a stream, converting the
        /// bytes into UTF8 encoded text.
        ///
        /// Does not read the entire stream like the StreamReader.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ReadLine(Stream s)
        {
            string ret = string.Empty;
            if (s.CanRead && s.Position < s.Length)
            {
                // read string one byte at a time until eof or eol
                byte[] sdata = new byte[10];
                int offsetIntoByteArray = 0;
                int charsToRead = 1;
                int cnt = s.Read(sdata, offsetIntoByteArray, charsToRead);
                if (cnt > 0)
                {
                    char[] cdata = Encoding.UTF8.GetChars(sdata);
                    // search for a new line or no more characters to read
                    while (cnt > 0 && cdata[0] != '\n')
                    {
                        if (cdata.GetLength(0) > 0)
                        {
                            for (int x = 0; x <= offsetIntoByteArray; ++x)
                            {
                                // ignore carriage return
                                if (cdata[x] != '\r')
                                {
                                    ret += cdata[x];
                                }
                            }
                            offsetIntoByteArray = 0;
                        }
                        else
                        {
                            offsetIntoByteArray += cnt;
                        }
                        cnt = s.Read(sdata, offsetIntoByteArray, charsToRead);
                        if (cnt > 0)
                        {
                            cdata = Encoding.UTF8.GetChars(sdata);
                        }
                    }
                }
            }
            _mLastLineRead = ret;
            return ret;
        }

        /// <summary>
        /// Deserialize one class from a given stream.
        /// The stream MUST have one line per Object. Which
        /// is what happens when this formatter was used
        /// to create the stream.
        /// </summary>
        /// <param name="s">Stream to deserialize</param>
        /// <returns></returns>
        public object Deserialize(Stream s)
        {
            object ret = null;
            // read one line from the stream that defines our object
            _mFirstLine = s.CanRead && s.Position == 0;
            string str = ReadLine(s);

            // local storage of the elements read from the line
            Dictionary<string, string> data
                = new Dictionary<string, string>();

            DeserializeFromString(str, Escape
                , NameDelimiter, ValueDelimiter, ref data);
            try
            {
                if (TextStorage == TextStorageType.Csv
                    && UseFirstLineAsColumnNames && _mFirstLine)
                { // create name map for this stream
                    foreach (KeyValuePair<string, string> value in data)
                    {
                        _mFirstLineColumnNames.Add(value.Value);
                    }
                    // return a copy of the name list
                    Type t = _mFirstLineColumnNames.GetType();
                    Type[] types = { };
                    object[] objs = { };
                    var constructorInfo = t.GetConstructor(types);
                    if (constructorInfo != null) ret = constructorInfo.Invoke(objs);
                }
                else
                {
                    // find relevant class to create and call its
                    // serialization constructor
                    Type t = ObjectType(data);

                    if (t != typeof(object))
                    {
                        // create a different SerializationInfo for each object
                        SerializationInfo info
                            = new SerializationInfo(t, _mConverter);
                        // add the name/value pairs to the SerializationInfo object
                        if (TextStorage == TextStorageType.NameValue)
                        {
                            foreach (KeyValuePair<string, string> value in data)
                            {
                                info.AddValue(value.Key, value.Value);
                            }
                        }
                        else if (TextStorage == TextStorageType.ValueOnly
                            || TextStorage == TextStorageType.Csv)
                        {
                            List<string> nameMap = ObjectNameMap(t);

                            IEnumerator<string> nameItr = null;
                            if (nameMap != null)
                            {
                                // starts before first item - MoveNext() to set at first item
                                nameItr = nameMap.GetEnumerator();
                            }
                            foreach (KeyValuePair<string, string> value in data)
                            {
                                if (nameMap != null)
                                {
                                    string key = string.Empty;
                                    if (nameItr.MoveNext())
                                    {
                                        if (nameItr.Current != null)
                                        {
                                            key = nameItr.Current;
                                        }
                                    }
                                    // an empty key from the name map
                                    // indicates to use the auto-generated key
                                    info.AddValue(key.Length > 0 ? key : value.Key, value.Value);
                                }
                                else
                                { // use the auto-generated key
                                    info.AddValue(value.Key, value.Value);
                                }
                            }
                        }

                        // call private constructor for ISerializable objects
                        Type[] types = { info.GetType(), _mContext.GetType() };
                        object[] objs = { info, _mContext };
                        var constructorInfo = t.GetConstructor(types);
                        if (constructorInfo != null) ret = constructorInfo.Invoke(objs);
                    }
                    else
                    {
                        // no type defined by the deserialized string
                    }
                }
            }
            catch (SerializationException e)
            {
                throw new SerializationException(nameof(Deserialize) + "(Stream)", e);
            }

            // return instance of new object
            return ret;
        }

        /// <summary>
        /// Deserialize to a specific type.
        /// This call bypasses the call to ObjectType(data)
        /// and uses the object type "t".
        /// </summary>
        /// <param name="s">Stream to deserialize</param>
        /// <param name="t">Object type to be created</param>
        /// <returns></returns>
        public object Deserialize(Stream s, Type t)
        {
            object ret = null;
            // read one line from the stream that defines our object
            _mFirstLine = s.CanRead && s.Position == 0;
            string str = ReadLine(s);

            // local storage of the elements read from the line
            Dictionary<string, string> data
                = new Dictionary<string, string>();

            DeserializeFromString(str, Escape
                , NameDelimiter, ValueDelimiter, ref data);
            try
            {
                if (TextStorage == TextStorageType.Csv
                    && UseFirstLineAsColumnNames && _mFirstLine)
                { // create name map for this stream
                    foreach (KeyValuePair<string, string> value in data)
                    {
                        _mFirstLineColumnNames.Add(value.Value);
                    }
                    // return a copy of the name list
                    t = _mFirstLineColumnNames.GetType();
                    Type[] types = { };
                    object[] objs = { };
                    var constructorInfo = t.GetConstructor(types);
                    if (constructorInfo != null) ret = constructorInfo.Invoke(objs);
                }
                else if (t != typeof(object))
                {
                    // create a different SerializationInfo for each object
                    SerializationInfo info
                        = new SerializationInfo(t, _mConverter);
                    // add the name/value pairs to the SerializationInfo object
                    if (TextStorage == TextStorageType.NameValue)
                    {
                        foreach (KeyValuePair<string, string> value in data)
                        {
                            info.AddValue(value.Key, value.Value);
                        }
                    }
                    else if (TextStorage == TextStorageType.ValueOnly
                        || TextStorage == TextStorageType.Csv)
                    {
                        List<string> nameMap = ObjectNameMap(t);
                        IEnumerator<string> nameItr = null;
                        if (nameMap != null)
                        {
                            // starts before first item - MoveNext() to set at first item
                            nameItr = nameMap.GetEnumerator();
                        }
                        foreach (KeyValuePair<string, string> value in data)
                        {
                            if (nameMap != null)
                            {
                                string key = string.Empty;
                                if (nameItr.MoveNext())
                                {
                                    if (nameItr.Current != null)
                                    {
                                        key = nameItr.Current;
                                    }
                                }
                                // an empty key from the name map
                                // indicates to use the auto-generated key
                                info.AddValue(key.Length > 0 ? key : value.Key, value.Value);
                            }
                            else
                            { // use the auto-generated key
                                info.AddValue(value.Key, value.Value);
                            }
                        }
                    }

                    // call private constructor for ISerializable objects
                    Type[] types = { info.GetType(), _mContext.GetType() };
                    object[] objs = { info, _mContext };
                    var constructorInfo = t.GetConstructor(types);
                    if (constructorInfo != null) ret = constructorInfo.Invoke(objs);
                }
                else
                {
                    // no type defined by the deserialized string
                }
            }
            catch (SerializationException e)
            {
                throw new SerializationException(nameof(Deserialize) + "(Stream, Type)", e);
            }
            // return instance of new object
            return ret;
        }

        /// <summary>
        /// Serialize an object to a stream.
        /// Objects are serialized only if they have the Serializable attribute
        /// and inherit from ISerializable.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="o"></param>
        public void Serialize(Stream s, object o)
        {
            try
            {
                // create a different SerializationInfo for each object
                Type t = o.GetType();
                SerializationInfo info
                    = new SerializationInfo(t, _mConverter);
                string classname = t.FullName;

                // first check if there is a serialization surrogate
                // for the object type
                ISerializationSurrogate surrogate = null;
                if (_mSelector != null)
                {
                    surrogate = _mSelector.GetSurrogate(t, _mContext, out _mSelector);
                }
                if (surrogate != null)
                {
                    // is this object serializable?
                    if (o is ISerializable)
                    {
                        surrogate.GetObjectData(o, info, _mContext);
                    }
                    else
                    {
                        // surrogate requires object to inherit from
                        // ISerializable interface
                        throw new SerializationException(
                            "Object is not serializable: " + t.FullName);
                    }
                }
                else
                {
                    // is this object serializable?
                    if (o is ISerializable)
                    {
                        ISerializable so = o as ISerializable;
                        so.GetObjectData(info, _mContext);
                    }
                    else
                    {
                        // we require objects to inherit from
                        // ISerializable interface
                        throw new SerializationException(
                            "Object is not serializable: " + t.FullName);
                    }
                }
                // now serialize the data to the stream
                // for each info value write to the stream
                // starting with the full class name
                string str = SerializeToString(info, t);
                //write the line to the stream
                StreamWriter sw = new StreamWriter(s);
                sw.WriteLine(str.ToString());
                sw.Flush();
            }
            catch (SerializationException e)
            {
                throw new SerializationException(nameof(Deserialize) + "(Stream, Object)", e);
            }
        }

        /// <summary>
        /// Child text formatter classes can create their own
        /// method to build the string to be written to the
        /// stream.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual string SerializeToString(SerializationInfo info, Type t)
        {
            StringBuilder str = new StringBuilder();
            if (TextStorage == TextStorageType.NameValue)
            {
                str.Append($"SYS_ClassFullName{ValueDelimiter}{t.FullName}");
            }
            else if (TextStorage == TextStorageType.ValueOnly)
            {
                str.AppendFormat("{0}", t.FullName);
            }
            foreach (SerializationEntry e in info)
            {
                if (TextStorage == TextStorageType.NameValue)
                {
                    if (e.Name != null)
                    {
                        string name = EscapeString(e.Name, Escape
                            , new char[] { NameDelimiter, ValueDelimiter });
                        if (e.Value != null)
                        {
                            string value = EscapeString(e.Value.ToString(), Escape
                                , new char[] { NameDelimiter, ValueDelimiter });
                            str.Append($"{NameDelimiter}{name}{ValueDelimiter}{value}");
                        }
                        else
                        {
                            str.AppendFormat("{0}{1}{2}{3}"
                                , NameDelimiter, name
                                , ValueDelimiter, string.Empty);
                        }
                    }
                }
                else if (TextStorage == TextStorageType.ValueOnly)
                {
                    if (e.Value != null)
                    {
                        string value = EscapeString(e.Value.ToString(), Escape
                            , new char[] { NameDelimiter });
                        str.Append($"{NameDelimiter}{value}");
                    }
                }
                else if (TextStorage == TextStorageType.Csv)
                {
                    if (e.Value != null)
                    {
                        string value = e.Value.ToString();
                        if (QuoteAll || value.IndexOfAny("\",\x0A\x0D".ToCharArray()) > -1)
                        {
                            value = "\"" + value.Replace("\"", "\"\"") + "\"";
                        }
                        str.Append($"{NameDelimiter}{value}");
                    }
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Binder is currently ignored by this class
        /// </summary>
        public SerializationBinder Binder { get; set; } = null;

        /// <summary>
        /// Indicates the source/destination stream type
        /// </summary>
        public StreamingContext Context
        {
            get { return _mContext; }
            set { _mContext = value; }
        }

        /// <summary>
        /// Type specific selection of a serialization object
        /// that can de/serialize a given type. The object must
        /// inherit from ISerializationSurrogate.  This facility
        /// is used to serialize objects that do not have the
        /// Serializable attribute or inherit from ISerializable.
        /// </summary>
        public ISurrogateSelector SurrogateSelector
        {
            get { return _mSelector; }
            set { _mSelector = value; }
        }

    }
}
