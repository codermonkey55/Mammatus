using System;
using System.Text;

namespace Mammatus.Extensions
{
    public static class BytesExtensions
    {
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }
        public static int BytesToInt32(byte[] data)
        {
            if (data.Length < 4)
            {
                return 0;
            }

            int num = 0;

            if (data.Length >= 4)
            {
                byte[] tempBuffer = new byte[4];

                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            return num;
        }
    }
}
