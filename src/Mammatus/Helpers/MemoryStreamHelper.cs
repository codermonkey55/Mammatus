using System.IO;
using System.Xml;

namespace Mammatus.Helpers
{
    public class MemoryStreamHelper
    {
        /// <summary>
        /// Removes the byte order mark.
        /// </summary>
        /// <param name="xml">The XML in bytes.</param>
        /// <param name="settings">The serialization settings.</param>
        /// <returns></returns>
        public string RemoveByteOrderMark(MemoryStream stream, XmlWriterSettings settings)
        {
            int bomIndex = settings.Encoding.GetPreamble().Length;
            byte[] xml = stream.ToArray();

            return settings.Encoding.GetString(xml, bomIndex, xml.Length - bomIndex);
        }
    }
}
