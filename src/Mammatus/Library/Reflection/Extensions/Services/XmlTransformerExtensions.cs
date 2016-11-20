﻿using Mammatus.Library.Reflection.Common;
using Mammatus.Library.Reflection.Extensions.Core;
using System;
using System.Text;

namespace Mammatus.Library.Reflection.Extensions.Services
{
    /// <summary>
    /// This class defines extensions for transforming any object to XML.
    /// </summary>
    public static class XmlTransformerExtensions
    {
        #region ToXml

        /// <summary>
        /// Generates a string representation of the given <paramref name="obj"/> using the default
        /// <see href="FormatOptions" />. The output will contain one element for every readable
        /// property on <paramref name="obj"/> and process reference properties (other than strings)
        /// recursively. This method does not handle cyclic references - passing in such an object
        /// graph will cause an infinite loop.
        /// </summary>
        /// <param name="obj">The object to convert to XML.</param>
        /// <returns>A string containing the generated XML data.</returns>
        public static string ToXml(this object obj)
        {
            return ToXml(obj, FormatOptions.Default);
        }

        /// <summary>
        /// Generates a string representation of the given <paramref name="obj"/> using the default
        /// <see href="FormatOptions" />. The output will contain one element for every readable
        /// property on <paramref name="obj"/> and process reference properties (other than strings)
        /// recursively. This method does not handle cyclic references - passing in such an object
        /// graph will cause an infinite loop.
        /// </summary>
        /// <param name="obj">The object to convert to XML.</param>
        /// <param name="options"></param>
        /// <returns>A string containing the generated XML data.</returns>
        public static string ToXml(this object obj, FormatOptions options)
        {
            bool newLineAfterElement = (options & FormatOptions.NewLineAfterElement) == FormatOptions.NewLineAfterElement;
            string afterElement = newLineAfterElement ? Environment.NewLine : String.Empty;
            bool tabIndent = (options & FormatOptions.UseSpaces) != FormatOptions.UseSpaces;
            string indent = tabIndent ? "\t" : "    ";
            bool addHeader = (options & FormatOptions.AddHeader) == FormatOptions.AddHeader;
            string header = addHeader ? "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine : string.Empty;
            return ToXml(obj, header, afterElement, indent, String.Empty);
        }

        #endregion

        #region ToXml Implementation

        private static string ToXml(object obj, string header, string afterElementDecoration,
                                     string indentDecoration, string currentIndent)
        {
            StringBuilder sb = new StringBuilder();
            Type type = obj.GetType();
            sb.Append(header);
            sb.AppendFormat("{0}<{1}>{2}", currentIndent, type.Name, afterElementDecoration);

            currentIndent = Indent(indentDecoration, currentIndent);
            // enumerate all instance properties
            foreach (var propertyInfo in type.Properties())
            {
                // ignore properties we cannot read
                if (propertyInfo.CanRead && propertyInfo.Name != "Item")
                {
                    object propertyValue = propertyInfo.Get(obj);
                    Type propertyType = propertyInfo.PropertyType;
                    if ((propertyType.IsClass || propertyType.IsInterface) && propertyType != typeof(string))
                    {
                    }
                    if ((propertyType.IsClass || propertyType.IsInterface) && propertyType != typeof(string))
                    {
                        sb.AppendFormat(ToXml(propertyValue, string.Empty, afterElementDecoration, indentDecoration, currentIndent));
                    }
                    else
                    {
                        sb.AppendFormat("{0}<{1}>{2}</{1}>{3}",
                                         currentIndent,
                                         propertyInfo.Name,
                                         propertyValue,
                                         afterElementDecoration);
                    }
                }
            }
            currentIndent = Unindent(indentDecoration, currentIndent);
            sb.AppendFormat("{0}</{1}>{2}", currentIndent, type.Name, afterElementDecoration);
            return sb.ToString();
        }

        private static string Indent(string indent, string currentIndent)
        {
            return currentIndent + indent;
        }

        private static string Unindent(string indent, string currentIndent)
        {
            return currentIndent.Substring(0, currentIndent.Length - indent.Length);
        }

        #endregion
    }
}
