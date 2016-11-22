using System;

namespace Mammatus.IO
{
    public static class FileFunctions
    {
        public static string GetFileName(string fileName)
        {
            int i = fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1;
            string Name = fileName.Substring(i);
            return Name;
        }

        public static string GetExtension(string fileName)
        {
            int i = fileName.LastIndexOf(".", StringComparison.Ordinal) + 1;
            string Name = fileName.Substring(i);
            return Name;
        }
    }
}
