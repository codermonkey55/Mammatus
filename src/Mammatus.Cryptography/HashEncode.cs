using System;
using System.Security.Cryptography;
using System.Text;

namespace Mammatus.Cryptography
{
    public class HashEncode
    {
        protected HashEncode()
        {
            //
            // TODO:
            //
        }

        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }

        public static string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }

        public static string HashEncoding(string Security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }
    }
}
