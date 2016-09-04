using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Mammatus.Helpers
{
    public sealed class ImageHelper
    {
        public static void CaptureScreen(Screen screen, string filename)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(screen.WorkingArea.Width, screen.WorkingArea.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(screen.WorkingArea.Location, new Point(0, 0), screen.WorkingArea.Size);
                        bitmap.Save(Path.GetFullPath(filename), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't capture screen - exception {e.GetType().FullName} encountered: {e.Message}");
            }
        }
    }
}
