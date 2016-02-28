using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Mammatus.Configuration.Elements
{
    public class DataProviderElement : ConfigurationElement
    {
        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty("dataProvider", IsRequired = false)]
        public Type DataProvider
        {
            get
            {
                return (Type)this["dataProvider"];
            }
        }

        [ConfigurationProperty("dataProviderName", IsRequired = false)]
        public string DataProviderName
        {
            get
            {
                return (string)this["dataProviderName"];
            }
        }

        [ConfigurationProperty("dataFolder", IsRequired = false)]
        public string DataFolder
        {
            get
            {
                return (string)this["dataFolder"];
            }
        }

        [ConfigurationProperty("connectionString", IsRequired = false)]
        public string ConnectionString
        {
            get
            {
                return (string)this["connectionString"];
            }
        }
    }
}
