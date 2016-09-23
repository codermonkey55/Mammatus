using System;
using System.Configuration;

namespace Mammatus.Configuration.Helpers
{
    public class ConfigurationManagerHelper
    {
        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetAppSetting(string key, string defaultValue)
        {
            string value = GetAppSetting(key);
            if (string.IsNullOrEmpty(value))
                value = defaultValue;

            return value;
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int GetAppSetting(string key, int defaultValue)
        {
            int ret = defaultValue;
            string value = GetAppSetting(key);
            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out ret))
                return defaultValue;

            return ret;
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public bool GetAppSetting(string key, bool defaultValue)
        {
            bool ret = defaultValue;
            string value = GetAppSetting(key);
            if (string.IsNullOrEmpty(value) || !bool.TryParse(value, out ret))
                return defaultValue;

            return ret;
        }

        /// <summary>
        /// Gets the application setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public Uri GetAppSetting(string key, Uri defaultValue)
        {
            Uri ret = defaultValue;
            string value = GetAppSetting(key);
            if (!string.IsNullOrEmpty(value))
                ret = new Uri(value);

            return ret;
        }
    }
}
