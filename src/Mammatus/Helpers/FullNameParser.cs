using System;
using System.Text.RegularExpressions;
using Mammatus.Models;

namespace Mammatus.Helpers
{
    public sealed class FullNameParser
    {
        /// <summary>
        /// Parses the specified full name.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Cannot parse name supplied.  Provide a valid space separated full name.;fullname</exception>
        public static Name Parse(string fullName)
        {
            Name name = new Name();

            string fullNamePattern = "^(?<title>.*\\.\\s)*(?<firstname>([A-Z][a-z]+\\s*)+)(\\s)(?<middleinitial>([A-Z]\\.?\\s)*)(?<lastname>[A-Z][a-zA-Z-']+)(?<suffix>.*)$";

            Regex regex = new Regex(fullNamePattern);

            MatchCollection matches = regex.Matches(fullName);

            if (matches.Count == 0) throw new ArgumentException("Cannot parse name supplied.  Provide a valid space separated full name.", "fullname");

            foreach (Match match in matches)
            {
                name.Title = match.Groups["title"].Value.Trim();

                name.First = match.Groups["firstname"].Value.Trim();

                name.Middle = match.Groups["middleinitial"].Value.Trim();

                name.Last = match.Groups["lastname"].Value.Trim();

                name.Suffix = match.Groups["suffix"].Value.Trim();
            }

            return name;
        }
    }

}
