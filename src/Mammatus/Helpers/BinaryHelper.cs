using System;

using System.Linq;

namespace Mammatus.Helpers
{
    using System.IO;

    /// <summary>
    /// Read/Write Binary data from/to FileStream
    /// </summary>
    public class BinaryHelper
    {
        /// <summary>
        /// Write Double value
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <param name="isBigEndian"></param>
        public static void WriteDouble(FileStream stream, double value, bool isBigEndian)
        {
            var byteArray = BitConverter.GetBytes(value);

            stream.Write((BitConverter.IsLittleEndian ^ isBigEndian) ? byteArray : byteArray.Reverse().ToArray(),
                0, byteArray.Length);
        }

        /// <summary>
        /// Write Int32 value
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <param name="isBigEndian"></param>
        public static void WriteInt32(FileStream stream, int value, bool isBigEndian)
        {
            var byteArray = BitConverter.GetBytes(value);

            stream.Write((BitConverter.IsLittleEndian ^ isBigEndian) ? byteArray : byteArray.Reverse().ToArray(),
                0, byteArray.Length);
        }

        /// <summary>
        /// Read Double value
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        public static double ReadDouble(FileStream stream, bool isBigEndian)
        {
            var length = sizeof(double);
            var buf = new byte[length];
            stream.Read(buf, 0, length);

            var value = BitConverter.ToDouble((BitConverter.IsLittleEndian ^ isBigEndian)
                ? buf : buf.Reverse().ToArray(), 0);

            return value;
        }

        /// <summary>
        /// Read Int32 value
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        public static int ReadInt32(FileStream stream, bool isBigEndian)
        {
            var length = sizeof(Int32);
            var buf = new byte[length];
            stream.Read(buf, 0, length);

            var value = BitConverter.ToInt32((BitConverter.IsLittleEndian ^ isBigEndian)
                ? buf : buf.Reverse().ToArray(), 0);

            return value;
        }

        public static int ToggleEndian(int value)
        {
            var byteArray = BitConverter.GetBytes(value);
            return BitConverter.ToInt32(byteArray.Reverse().ToArray(), 0);
        }

        public static uint ReadIInt32(Stream stream, bool inBigEndian)
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, 4);

            if (BitConverter.IsLittleEndian ^ inBigEndian)
            {
                return BitConverter.ToUInt32(buffer, 0);
            }
            else
            {
                return BitConverter.ToUInt32(Reverse(buffer), 0);

            }
        }
        public static int ReadInt32(Stream stream, bool inBigEndian)
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, 4);

            if (BitConverter.IsLittleEndian ^ inBigEndian)
            {
                return BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                return BitConverter.ToInt32(Reverse(buffer), 0);
            }
        }

        public static int ReadInt16(Stream stream, bool inBigEndian)
        {
            var buffer = new byte[4];
            var buffer2 = new byte[2];
            stream.Read(buffer2, 0, 2);

            if (BitConverter.IsLittleEndian && !inBigEndian)
            {
                buffer[0] = buffer2[0];
                buffer[1] = buffer2[1];
                return BitConverter.ToInt32(buffer, 0);
            }
            else if (BitConverter.IsLittleEndian && inBigEndian)
            {
                buffer[1] = buffer2[0];
                buffer[0] = buffer2[1];
                return BitConverter.ToInt32(buffer, 0);
            }
            else if (!BitConverter.IsLittleEndian && !inBigEndian)
            {
                buffer[3] = buffer2[0];
                buffer[2] = buffer2[1];
                return BitConverter.ToInt32(buffer, 0);
            }
            else// if (!BitConverter.IsLittleEndian && inBigEndian)
            {
                buffer[2] = buffer2[0];
                buffer[3] = buffer2[1];
                return BitConverter.ToInt32(buffer, 0);
            }
        }

        public static int ReadIntMain(Stream stream, bool inBigEndian, int length)
        {
            var buffer = new byte[4];
            stream.Read(buffer, 0, length);

            if (BitConverter.IsLittleEndian && !inBigEndian)
            {
                return BitConverter.ToInt32(buffer, 0);
            }
            else
            {
                return BitConverter.ToInt32(Reverse(buffer), 0);
            }
        }

        private static byte[] Reverse(byte[] source)
        {
            var reversed = new byte[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                reversed[i] = source[source.Length - 1 - i];
            }
            return reversed;
        }

    }
}
