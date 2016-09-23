using System.Linq;
using System.Text;
using Mammatus.Extensions;

namespace Mammatus.Helpers
{
    using System.IO;

    public sealed class TextFileLoader
    {
        public static string Load(string filepath)
        {
            string text = "";

            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                byte[] bs = new byte[fs.Length];

                fs.Read(bs, 0, bs.Length);

                var enc = bs.GetCode();

                if (Equals(enc, Encoding.UTF8) && bs.Length > 3
                    && bs[0] == 0xEF && bs[1] == 0xBB && bs[2] == 0xBF)
                {
                    text = enc.GetString(bs.Skip(3).ToArray());
                }
                else
                {
                    text = enc.GetString(bs);
                }
            }
            return text;
        }
    }
}
