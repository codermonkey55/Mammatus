using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace Mammatus.Library.Image
{
    public static class PicDeal
    {
        private static Hashtable htmimes = new Hashtable();
        internal static readonly string AllowExt = ".jpe|.jpeg|.jpg|.png|.tif|.tiff|.bmp";

        static PicDeal()
        {
            SetImgType();
        }

        public static void AddWaterPic(string Path, string Path_sypf)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
                System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
                Graphics g = Graphics.FromImage(image);
                g.DrawImage(copyImage,
                    new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height,
                        copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height,
                    System.Drawing.GraphicsUnit.Pixel);
                g.Dispose();

                image.Save(Path + ".temp");
                image.Dispose();
                System.IO.File.Delete(Path);
                File.Move(Path + ".temp", Path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void SetImgType()
        {
            htmimes[".jpe"] = "image/jpeg";
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
        }

        public static Size ResizeImage(int width, int height, int maxWidth, int maxHeight)
        {
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal MAX_HEIGHT = (decimal)maxHeight;
            decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

            int newWidth, newHeight;

            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;
                // determine the largest factor
                if (originalWidth / originalHeight > ASPECT_RATIO)
                {
                    factor = originalWidth / MAX_WIDTH;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
            }

            return new Size(newWidth, newHeight);
        }
    }
}
