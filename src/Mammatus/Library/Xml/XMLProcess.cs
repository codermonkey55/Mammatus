using System;
using System.Data;
using System.IO;
using System.Xml;

namespace Mammatus.Library.Xml
{
    public class XmlProcess
    {
        public XmlProcess()
        { }

        public XmlProcess(string strPath)
        {
            this.XmlPath = strPath;
        }

        public string XmlPath { get; }

        private XmlDocument XmlLoad()
        {
            string xmlFile = XmlPath;
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + xmlFile;
                if (File.Exists(filename)) xmldoc.Load(filename);
            }
            catch (Exception)
            {
                throw;
            }
            return xmldoc;
        }

        private static XmlDocument XmlLoad(string strPath)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + strPath;
                if (File.Exists(filename)) xmldoc.Load(filename);
            }
            catch (Exception)
            {
                throw;
            }
            return xmldoc;
        }

        private static string GetXmlFullPath(string strPath)
        {
            if (strPath.IndexOf(":", StringComparison.Ordinal) > 0)
            {
                return strPath;
            }
            else
            {
                return System.Web.HttpContext.Current.Server.MapPath(strPath);
            }
        }

        public string Read(string node)
        {
            string value = "";
            try
            {
                XmlDocument doc = XmlLoad();
                XmlNode xn = doc.SelectSingleNode(node);
                value = xn.InnerText;
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }

        public static string Read(string path, string node)
        {
            string value = "";
            try
            {
                XmlDocument doc = XmlLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = xn.InnerText;
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }

        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = XmlLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }

        public string[] ReadAllChildallValue(string node)
        {
            int i = 0;
            string[] str = { };
            XmlDocument doc = XmlLoad();
            XmlNode xn = doc.SelectSingleNode(node);
            XmlNodeList nodelist = xn.ChildNodes;
            if (nodelist.Count > 0)
            {
                str = new string[nodelist.Count];
                foreach (XmlElement el in nodelist)
                {
                    str[i] = el.Value;
                    i++;
                }
            }
            return str;
        }

        public XmlNodeList ReadAllChild(string node)
        {
            XmlDocument doc = XmlLoad();
            XmlNode xn = doc.SelectSingleNode(node);
            XmlNodeList nodelist = xn.ChildNodes;
            return nodelist;
        }

        public DataView GetDataViewByXml(string strWhere, string strSort)
        {
            try
            {
                string xmlFile = this.XmlPath;
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + xmlFile;
                DataSet ds = new DataSet();
                ds.ReadXml(filename);
                DataView dv = new DataView(ds.Tables[0]);
                if (strSort != null)
                {
                    dv.Sort = strSort;
                }
                if (strWhere != null)
                {
                    dv.RowFilter = strWhere;
                }
                return dv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet GetDataSetByXml(string strXmlPath)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Insert(string path, string node, string element, string[][] strList)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = doc.CreateElement(element);
                string strAttribute = "";
                string strValue = "";
                for (int i = 0; i < strList.Length; i++)
                {
                    for (int j = 0; j < strList[i].Length; j++)
                    {
                        if (j == 0)
                            strAttribute = strList[i][j];
                        else
                            strValue = strList[i][j];
                    }
                    if (strAttribute.Equals(""))
                        xe.InnerText = strValue;
                    else
                        xe.SetAttribute(strAttribute, strValue);
                }
                xn.AppendChild(xe);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool WriteXmlByDataSet(string strXmlPath, string[] columns, string[] columnValue)
        {
            try
            {
                string strXsdPath = strXmlPath.Substring(0, strXmlPath.IndexOf(".")) + ".xsd";
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(GetXmlFullPath(strXsdPath));
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                DataTable dt = ds.Tables[0];
                DataRow newRow = dt.NewRow();
                for (int i = 0; i < columns.Length; i++)
                {
                    newRow[columns[i]] = columnValue[i];
                }
                dt.Rows.Add(newRow);
                dt.AcceptChanges();
                ds.AcceptChanges();
                ds.WriteXml(GetXmlFullPath(strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Update(string node, string value)
        {
            try
            {
                XmlDocument doc = XmlLoad();
                XmlNode xn = doc.SelectSingleNode(node);
                xn.InnerText = value;
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + XmlPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(string path, string node, string value)
        {
            try
            {
                XmlDocument doc = XmlLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                xn.InnerText = value;
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = XmlLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool UpdateXmlRow(string strXmlPath, string[] columns, string[] columnValue, string strWhereColumnName, string strWhereColumnValue)
        {
            try
            {
                string strXsdPath = strXmlPath.Substring(0, strXmlPath.IndexOf(".")) + ".xsd";
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(GetXmlFullPath(strXsdPath));
                ds.ReadXml(GetXmlFullPath(strXmlPath));


                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        if (ds.Tables[0].Rows[i][strWhereColumnName].ToString().Trim().Equals(strWhereColumnValue))
                        {

                            for (int j = 0; j < columns.Length; j++)
                            {
                                ds.Tables[0].Rows[i][columns[j]] = columnValue[j];
                            }
                            ds.AcceptChanges();
                            ds.WriteXml(GetXmlFullPath(strXmlPath));
                            return true;
                        }
                    }

                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void Delete(string path, string node)
        {
            try
            {
                XmlDocument doc = XmlLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                xn.ParentNode.RemoveChild(xn);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = XmlLoad(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DeleteXmlAllRows(string strXmlPath)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                }
                ds.WriteXml(GetXmlFullPath(strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteXmlRowByIndex(string strXmlPath, int iDeleteRow)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].Rows[iDeleteRow].Delete();
                }
                ds.WriteXml(GetXmlFullPath(strXmlPath));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteXmlRows(string strXmlPath, string strColumn, string[] columnValue)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(GetXmlFullPath(strXmlPath));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (columnValue.Length > ds.Tables[0].Rows.Count)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < columnValue.Length; j++)
                            {
                                if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(columnValue[j]))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < columnValue.Length; j++)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(columnValue[j]))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                            }
                        }
                    }
                    ds.WriteXml(GetXmlFullPath(strXmlPath));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}