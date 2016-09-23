using System.Threading;

namespace Mammatus.Library.Random
{
    using System;

    public class RandomGenerator
    {
        private int _rep;

        public static string GenerateNumber(int length)
        {
            return GenerateNumber(length, false);
        }

        public static string GenerateNumber(int length, bool sleep)
        {
            if (sleep) Thread.Sleep(3);
            string result = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }


        public static string GenerateAlphaNumeric(int length)
        {
            return GenerateAlphaNumeric(length, false);
        }

        public static string GenerateAlphaNumeric(int length, bool sleep)
        {
            if (sleep) Thread.Sleep(3);
            char[] pattern = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }


        public static string GenerateString(int length)
        {
            return GenerateString(length, false);
        }

        public static string GenerateString(int length, bool sleep)
        {
            if (sleep) Thread.Sleep(3);
            char[] pattern = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < length; i++)
            {
                int rnd = random.Next(0, n);
                result += pattern[rnd];
            }
            return result;
        }

        public string GenerateNumericCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + _rep;
            _rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> _rep)));
            for (int i = 0; i < codeCount; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10))));
            }
            return str;
        }

        public string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + _rep;
            _rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> _rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch;
            }
            return str;
        }

    }
}
