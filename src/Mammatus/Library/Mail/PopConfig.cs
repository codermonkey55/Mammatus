using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;

namespace Mammatus.Library.Mail
{

    public class PopConfig
    {
        private static PopConfig _popConfig;

        private string ConfigFile
        {
            get
            {
                string configPath = ConfigurationManager.AppSettings["PopConfigPath"];
                if (string.IsNullOrEmpty(configPath) || configPath.Trim().Length == 0)
                {
                    configPath = HttpContext.Current.Request.MapPath("/Config/PopSetting.config");
                }
                else
                {
                    if (!Path.IsPathRooted(configPath))
                        configPath = HttpContext.Current.Request.MapPath(Path.Combine(configPath, "PopSetting.config"));
                    else
                        configPath = Path.Combine(configPath, "PopSetting.config");
                }
                return configPath;
            }
        }

        public PopSetting PopSetting
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.ConfigFile);
                PopSetting popSetting = new PopSetting();
                popSetting.Server = doc.DocumentElement.SelectSingleNode("Server").InnerText;
                popSetting.Port = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("Port").InnerText);
                popSetting.UseSSL = Convert.ToBoolean(doc.DocumentElement.SelectSingleNode("UseSSL").InnerText);
                popSetting.UserName = doc.DocumentElement.SelectSingleNode("User").InnerText;
                popSetting.Password = doc.DocumentElement.SelectSingleNode("Password").InnerText;


                return popSetting;
            }


        }

        //public static Save()
        //{
        //}

        public static PopConfig Create()
        {
            if (_popConfig == null)
            {
                _popConfig = new PopConfig();
            }
            return _popConfig;
        }
    }

}