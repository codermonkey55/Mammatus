using System;
using System.IO;

namespace Mammatus.Validation
{
    public sealed class FilePathValidator
    {
        private FilePathValidator()
        {

        }

        public static string ValidatePath(string basePath, string mappedPath)
        {
            bool valid = false;

            try
            {
                // Check that we are indeed within the storage directory boundaries
                valid = Path.GetFullPath(mappedPath).StartsWith(Path.GetFullPath(basePath), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                // Make sure that if invalid for medium trust we give a proper exception
                valid = false;
            }

            if (!valid)
            {
                throw new ArgumentException("Invalid path");
            }

            return mappedPath;
        }
    }
}
