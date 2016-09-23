using System.Drawing;

namespace Mammatus.Library.Image
{
    using System;

    public class ValidateImg
    {
        public void GenerateValidateImg()
        {
            char[] chars = "023456789".ToCharArray();
            Random random = new Random();

            string validateCode = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                char rc = chars[random.Next(0, chars.Length)];
                if (validateCode.IndexOf(rc) > -1)
                {
                    i--;
                    continue;
                }
                validateCode += rc;
            }

            CreateImage(validateCode);
        }

        public Bitmap CreateImage(string checkCode)
        {
            Bitmap image = default(Bitmap);
            Graphics g = default(Graphics);
            try
            {
                int iwidth = (int)(checkCode.Length * 11);
                image = new Bitmap(iwidth, 19);
                g = Graphics.FromImage(image);
                g.Clear(Color.White);

                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Chocolate, Color.Brown, Color.DarkCyan, Color.Purple };
                Random rand = new Random();

                for (int i = 0; i < checkCode.Length; i++)
                {
                    int cindex = rand.Next(7);
                    Font f = new Font("Microsoft Sans Serif", 11);
                    Brush b = new SolidBrush(c[cindex]);
                    g.DrawString(checkCode.Substring(i, 1), f, b, (i * 10) + 1, 0, StringFormat.GenericDefault);
                }

                g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                //Response.ClearContent();
                //Response.ContentType = "image/Jpeg";
                //Response.BinaryWrite(ms.ToArray());

                return image;
            }
            finally
            {
                g?.Dispose();
                image?.Dispose();
            }




        }
    }
}
