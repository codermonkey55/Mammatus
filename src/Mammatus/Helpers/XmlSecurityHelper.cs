using System.Xml;

namespace Mammatus.Helpers
{
    /// <summary>
    /// Helper class to implement security on xml objects.
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/15/2009
    /// Notes:
    /// </remarks>
    public static class XmlSecurityHelper
    {
        /// <summary>
        /// Add settings to control expansion attack.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/15/2009
        /// Notes:
        /// http://blogs.msdn.com/tomholl/archive/2009/05/21/protecting-against-xml-entity-expansion-attacks.aspx
        /// "check that a document is well-formed, contains no DTDs (and hence no entity definitions) and is less than 10K in size"
        /// </remarks>
        public static void ControlExpansionAttack(XmlReaderSettings settings)
        {
            settings.XmlResolver = null;
            settings.MaxCharactersInDocument = 10000;
        }
    }
}
