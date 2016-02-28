using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using Mammatus.Configuration.Elements;

namespace Mammatus.Configuration.Sections
{
    public class MammatusConfigurationSection : ConfigurationSection
    {
        public const string SectinoName = "mammatus";

        public const string DataProviderElementName = "dataProvider";

        [ConfigurationProperty(DataProviderElementName, IsRequired = false)]
        public DataProviderElement DataProvider
        {
            get { return (DataProviderElement)base[DataProviderElementName]; }
            set { base[DataProviderElementName] = value; }
        }

        public static MammatusConfigurationSection GetMammatusConfigurationSection()
        {
            return (MammatusConfigurationSection)ConfigurationManager.GetSection(SectinoName);
        }
    }
}
