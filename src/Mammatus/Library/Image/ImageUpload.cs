using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Mammatus.Library.Image
{
    public class ImageUpload
    {
        private int _maxSize = 1024 * 1024;
        private string _fileType = "jpg;gif;bmp;png";
        private string _savePath = System.Web.HttpContext.Current.Server.MapPath(".") + "\\";
        private int _saveType = 0;
        private HtmlInputFile _formFile;
        private string _inFileName = "";
        private string _copyIamgePath = System.Web.HttpContext.Current.Server.MapPath(".") + "/images/5dm_new.jpg";

        public int Error { get; private set; } = 0;

        public int MaxSize
        {
            set { _maxSize = value; }
        }

        public string FileType
        {
            set { _fileType = value; }
        }

        public string SavePath
        {
            set { _savePath = System.Web.HttpContext.Current.Server.MapPath(value); }
            get { return _savePath; }
        }

        public int SaveType
        {
            set { _saveType = value; }
        }

        public HtmlInputFile FormFile
        {
            set { _formFile = value; }
        }

        public string InFileName
        {
            set { _inFileName = value; }
        }

        public string OutFileName { get; set; } = "";

        public string OutThumbFileName
        {
            get;
            set;
        }

        public bool Iss { get; private set; } = false;

        public int Width { get; private set; } = 0;

        public int Height { get; private set; } = 0;

        public int SWidth { get; set; } = 120;

        public int SHeight { get; set; } = 120;

        public bool IsCreateImg { get; set; } = true;

        public bool IsDraw { get; set; } = false;

        public int DrawStyle { get; set; } = 0;

        public int DrawStringX { get; set; } = 10;

        public int DrawStringY { get; set; } = 10;

        public string AddText { get; set; } = "GlobalNatureCrafts";

        public string Font { get; set; } = "";

        public int FontSize { get; set; } = 12;

        public int FileSize { get; set; } = 0;

        public string CopyIamgePath
        {
            set { _copyIamgePath = System.Web.HttpContext.Current.Server.MapPath(value); }
        }

        private string GetExt(string path)
        {
            return Path.GetExtension(path);
        }

        private string FileName(string ext)
        {
            if (_saveType == 0 || _inFileName.Trim() == "")
                return DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
            else
                return _inFileName;
        }

        private bool IsUpload(string ext)
        {
            ext = ext.Replace(".", "");
            bool b = false;
            string[] arrFileType = _fileType.Split(';');
            foreach (string str in arrFileType)
            {
                if (str.ToLower() == ext.ToLower())
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        public void Upload()
        {
            HttpPostedFile hpFile = _formFile.PostedFile;
            if (hpFile == null || hpFile.FileName.Trim() == "")
            {
                Error = 1;
                return;
            }
            string ext = GetExt(hpFile.FileName);
            if (!IsUpload(ext))
            {
                Error = 2;
                return;
            }
            int iLen = hpFile.ContentLength;
            if (iLen > _maxSize)
            {
                Error = 3;
                return;
            }
            try
            {
                if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
                byte[] bData = new byte[iLen];
                hpFile.InputStream.Read(bData, 0, iLen);
                string fName;
                fName = FileName(ext);
                string tempFile = "";
                if (IsDraw)
                {
                    tempFile = fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString();
                }
                else
                {
                    tempFile = fName;
                }
                FileStream newFile = new FileStream(_savePath + tempFile, FileMode.Create);
                newFile.Write(bData, 0, bData.Length);
                newFile.Flush();
                int fileSizeTemp = hpFile.ContentLength;

                string imageFilePath = _savePath + fName;
                if (IsDraw)
                {
                    if (DrawStyle == 0)
                    {
                        System.Drawing.Image img1 = System.Drawing.Image.FromStream(newFile);
                        Graphics g = Graphics.FromImage(img1);
                        g.DrawImage(img1, 100, 100, img1.Width, img1.Height);
                        Font f = new Font(Font, FontSize);
                        Brush b = new SolidBrush(Color.Red);
                        string addtext = AddText;
                        g.DrawString(addtext, f, b, DrawStringX, DrawStringY);
                        g.Dispose();
                        img1.Save(imageFilePath);
                        img1.Dispose();
                    }
                    else
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(newFile);
                        System.Drawing.Image copyImage = System.Drawing.Image.FromFile(_copyIamgePath);
                        Graphics g = Graphics.FromImage(image);
                        g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width - 5, image.Height - copyImage.Height - 5, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        g.Dispose();
                        image.Save(imageFilePath);
                        image.Dispose();
                    }
                }

                //获取图片的高度和宽度
                System.Drawing.Image img = System.Drawing.Image.FromStream(newFile);
                Width = img.Width;
                Height = img.Height;

                //生成缩略图部分
                if (IsCreateImg)
                {
                    #region 缩略图大小只设置了最大范围，并不是实际大小
                    float realbili = (float)Width / (float)Height;
                    float wishbili = (float)SWidth / (float)SHeight;

                    //实际图比缩略图最大尺寸更宽矮，以宽为准
                    if (realbili > wishbili)
                    {
                        SHeight = (int)((float)SWidth / realbili);
                    }
                    //实际图比缩略图最大尺寸更高长，以高为准
                    else
                    {
                        SWidth = (int)((float)SHeight * realbili);
                    }
                    #endregion

                    this.OutThumbFileName = fName.Split('.').GetValue(0).ToString() + "_s." + fName.Split('.').GetValue(1).ToString();
                    string imgFilePath = _savePath + this.OutThumbFileName;

                    System.Drawing.Image newImg = img.GetThumbnailImage(SWidth, SHeight, null, System.IntPtr.Zero);
                    newImg.Save(imgFilePath);
                    newImg.Dispose();
                    Iss = true;
                }

                if (IsDraw)
                {
                    if (File.Exists(_savePath + fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString()))
                    {
                        newFile.Dispose();
                        File.Delete(_savePath + fName.Split('.').GetValue(0).ToString() + "_temp." + fName.Split('.').GetValue(1).ToString());
                    }
                }
                newFile.Close();
                newFile.Dispose();
                OutFileName = fName;
                FileSize = fileSizeTemp;
                Error = 0;
                return;
            }
            catch (Exception ex)
            {
                Error = 4;
                return;
            }
        }
    }
}