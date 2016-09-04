namespace Mammatus.Helpers
{
    public static class FileSizeConverter
    {
        private const double Threshold = 1024;

        public static string ConvertFrom(double size)
        {
            return ConvertFrom(size, "#,##0.##");
        }

        public static string ConvertFrom(double size, string specifier)
        {
            string[] suffix = { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };

            int index = 0;

            while (size >= Threshold)
            {
                size /= Threshold;
                index++;
            }

            return $"{size.ToString(specifier)} {(index < suffix.Length ? suffix[index] : "-")}B";
        }

        public static string ConvertAuto(double size)
        {
            string[] suffix = { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
            int index = 0;


            while (size >= 1000)
            {
                size /= Threshold;
                index++;
            }

            if (size < 10)
            {
                return $"{size.ToString("#,##0.00")} {(index < suffix.Length ? suffix[index] : "-")}B";
            }
            if (size < 100)
            {
                return $"{size.ToString("#,##0.0")} {(index < suffix.Length ? suffix[index] : "-")}B";
            }

            return $"{size.ToString("#,##0")} {(index < suffix.Length ? suffix[index] : "-")}B";
        }
    }
}
