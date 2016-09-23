namespace Mammatus.Enums
{
    /// <summary>
    /// The storage type indicates how the text is formatted
    /// when written to the stream and the expected format
    /// when reading from the stream.
    /// </summary>
    public enum TextStorageType
    {
        NameValue, // Name1=Value,Name2=Value,...
        ValueOnly, // Value,Value,...
        Csv // CSV format follows the standard RFC 4180.
    }

}