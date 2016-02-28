using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mammatus.Stream.Extensions
{
    public static class StreamExtentions
    {
        public static byte[] ToBytes(this System.IO.Stream inputStream)
        {
            if (null == inputStream)
            {
                return new byte[0];
            }

            byte[] bytes = new byte[inputStream.Length];

            inputStream.Read(bytes, 0, bytes.Length);

            inputStream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }

        public static System.IO.Stream ToStream(this byte[] bytes)
        {
            if (null == bytes)
            {
                return null;
            }

            return new MemoryStream(bytes);
        }
    }
}
