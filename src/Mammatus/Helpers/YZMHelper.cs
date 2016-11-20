using Mammatus.Library.Random;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Web;

namespace Mammatus.Helpers
{
    public class YzmHelper
    {
        private readonly string _text;
        private Bitmap _image;
        private int _letterCount = 4;
        private int _letterWidth = 16;
        private int _letterHeight = 20;
        private static readonly byte[] Randb = new byte[4];
        private static readonly RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();
        private readonly Font[] _fonts =
        {
           new Font(new FontFamily("Times New Roman"),10 +Next(1),FontStyle.Regular),
           new Font(new FontFamily("Georgia"), 10 + Next(1),FontStyle.Regular),
           new Font(new FontFamily("Arial"), 10 + Next(1),FontStyle.Regular),
           new Font(new FontFamily("Comic Sans MS"), 10 + Next(1),FontStyle.Regular)
        };

        public string Text => _text;

        public Bitmap Image => _image;

        public YzmHelper()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
            _text = RandomGenerator.GenerateNumber(4);
            CreateImage();
        }

        private static int Next(int max)
        {
            Rand.GetBytes(Randb);
            int value = BitConverter.ToInt32(Randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }

        public void CreateImage()
        {
            int intImageWidth = _text.Length * _letterWidth;
            Bitmap image = new Bitmap(intImageWidth, _letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = Next(image.Width - 1);
                int x2 = Next(image.Width - 1);
                int y1 = Next(image.Height - 1);
                int y2 = Next(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            int _x = -12, _y = 0;
            for (int intIndex = 0; intIndex < _text.Length; intIndex++)
            {
                _x += Next(12, 16);
                _y = Next(-2, 2);
                string strChar = _text.Substring(intIndex, 1);
                strChar = Next(1) == 1 ? strChar.ToLower() : strChar.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(strChar, _fonts[Next(_fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = Next(image.Width - 1);
                int y = Next(image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
            }
            image = TwistImage(image, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, intImageWidth - 1, (_letterHeight - 1));
            _image = image;
        }

        public Color GetRandomColor()
        {
            Random randomNumFirst = new Random((int)DateTime.Now.Ticks);
            Thread.Sleep(randomNumFirst.Next(50));
            Random randomNumSencond = new Random((int)DateTime.Now.Ticks);
            int intRed = randomNumFirst.Next(180);
            int intGreen = randomNumSencond.Next(180);
            int intBlue = (intRed + intGreen > 300) ? 0 : 400 - intRed - intGreen;
            intBlue = (intBlue > 255) ? 255 : intBlue;
            return Color.FromArgb(intRed, intGreen, intBlue);
        }

        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double pi = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    var dx = bXDir ? (pi * j) / dBaseAxisLen : (pi * i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    var nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    var nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
    }
}