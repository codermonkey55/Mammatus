using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Mammatus.Helpers
{
    public static class XmlSerializationHelper
    {
        /// <summary>
        /// Serializes the specified object using Unicode encoding
        /// </summary>
        /// <typeparam name="T">The type of object being serialized</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>Xml string</returns>
        public static string Serialize<T>(T obj)
        {
            return Serialize<T>(obj, Encoding.Unicode);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object being serialized</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="encoding">The encoding with which to serialize</param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, Encoding encoding)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = encoding
            };

            return Serialize<T>(obj, settings);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object being serialized</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="settings">The serialization settings.  Can be used to omit the declaration header.</param>
        /// <returns>Xml string</returns>
        public static string Serialize<T>(T obj, XmlWriterSettings settings)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlTextWriter.Create(stream, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, obj);

                    return settings.Encoding.GetString(stream.ToArray());
                };
            }
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="settings">The serialization settings.  Can be used to omit the declaration header.</param>
        /// <param name="streamHelper">The MemoryStream helper.  Can be used to remove the byte order mark</param>
        /// <returns>Xml string</returns>
        public static string Serialize<T>(T obj, XmlWriterSettings settings, MemoryStreamHelper streamHelper)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlTextWriter.Create(stream, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, obj);

                    return streamHelper.RemoveByteOrderMark(stream, settings);
                };
            }
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <typeparam name="T">The object type to which the xml will be transformed</typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns>Typed object</returns>
        public static T Deserialize<T>(string xml)
        {
            return Deserialize<T>(xml, Encoding.Unicode);
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <typeparam name="T">The object type to which the xml will be transformed</typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="encoding">The encoding to use for deserialization.</param>
        /// <returns>Typed object</returns>
        public static T Deserialize<T>(string xml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(xml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <typeparam name="T">The object type to which the xml will be transformed</typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="encoding">The encoding to use for deserialization.</param>
        /// <param name="xmlRoot">The XML root element to use.</param>
        /// <returns>Typed object</returns>
        public static T Deserialize<T>(string xml, Encoding encoding, XmlRootAttribute xmlRoot)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(xml)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
