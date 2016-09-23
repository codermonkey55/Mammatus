using System.Data;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Mammatus.Library.Xml
{
    public class XmlData
    {
        /// <summary>
        /// Gets the XML document.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static IXPathNavigable GetXmlDocument(DataTable table)
        {
            XmlDocument doc = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                table.WriteXml(ms, XmlWriteMode.IgnoreSchema);
                ms.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;

                XmlReader reader = XmlReader.Create(ms, settings);

                doc.Load(reader);
            }
            return doc;
        }
    }
}
