﻿using System;

namespace Mammatus.Library.Reflection.Common
{
    /// <summary>
    /// This enumeration allows you to customize the XML output of the ToXml extensions.
    /// </summary>
    [Flags]
    public enum FormatOptions
    {
        /// <summary>
        /// This option specifies the empty set of options and does not affect the output.
        /// </summary>
        None = 0,
        /// <summary>
        /// If this option is specified the generated XML will include an XML document header.
        /// </summary>
        AddHeader = 1,
        /// <summary>
        /// If this option is specified a line feed will be emitted after every XML element.
        /// </summary>
        NewLineAfterElement = 2,
        /// <summary>
        /// If this option is specified nested tags will be indented either 1 tab character
        /// (the default) or 4 space characters.
        /// </summary>
        Indent = 4,
        /// <summary>
        /// If this option is specified indentation will use spaces instead of tabs.
        /// </summary>
        UseSpaces = 8,
        /// <summary>
        /// This option, which combines AddHeader, NewLineAfterElement and Indent, provides the
        /// default set of options used.
        /// </summary>
        Default = AddHeader | NewLineAfterElement | Indent
    }
}
