using Mammatus.Helpers;
using Mammatus.IO;
using System.Data;
using System.Xml;

namespace Mammatus.Library.Xml
{
    public class XmlUtil
    {
        private readonly string _filePath;

        private XmlDocument _xml;

        private XmlElement _element;

        public XmlUtil(string xmlFilePath)
        {
            _filePath = SystemHelper.GetPath(xmlFilePath);
        }

        private void CreateXmlElement()
        {
            _xml = new XmlDocument();

            if (DirFile.IsExistFile(_filePath))
            {
                _xml.Load(this._filePath);
            }
            _element = _xml.DocumentElement;
        }

        public XmlNode GetNode(string xPath)
        {
            CreateXmlElement();

            return _element.SelectSingleNode(xPath);
        }

        public string GetValue(string xPath)
        {
            CreateXmlElement();

            return _element.SelectSingleNode(xPath).InnerText;
        }

        public string GetAttributeValue(string xPath, string attributeName)
        {
            CreateXmlElement();

            return _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        public void AppendNode(XmlNode xmlNode)
        {
            CreateXmlElement();

            XmlNode node = _xml.ImportNode(xmlNode, true);

            _element.AppendChild(node);
        }

        public void AppendNode(DataSet ds)
        {
            XmlDataDocument xmlDataDocument = new XmlDataDocument(ds);

            XmlNode node = xmlDataDocument.DocumentElement.FirstChild;

            AppendNode(node);
        }

        public void RemoveNode(string xPath)
        {
            CreateXmlElement();

            XmlNode node = _xml.SelectSingleNode(xPath);

            _element.RemoveChild(node);
        }

        public void Save()
        {
            CreateXmlElement();

            _xml.Save(this._filePath);
        }

        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            string filePath = "";

            filePath = SystemHelper.GetPath(xmlFilePath);

            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(filePath);

            return xmlDocument.DocumentElement;
        }

        public static string GetValue(string xmlFilePath, string xPath)
        {
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            return rootElement.SelectSingleNode(xPath).InnerText;
        }

        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        public static void SetValue(string xmlFilePath, string xPath, string newtext)
        {
            //string path = SystemHelper.GetPath(xmlFilePath);
            //var queryXML = from xmlLog in xelem.Descendants("msg_log")
            //               where xmlLog.Element("user").Value == "Bin"
            //               select xmlLog;

            //foreach (XElement el in queryXML)
            //{
            //    el.Element("user").Value = "LiuBin";
            //}
            //xelem.Save(path);
        }
    }
}
