using System;
using System.IO;
using System.Web;

namespace Mammatus.Library.Image
{
    public static class FileValidation
    {
        public static bool IsAllowedExtension(HttpPostedFile oFile, FileExtension[] fileEx)
        {
            int fileLen = oFile.ContentLength;
            byte[] imgArray = new byte[fileLen];
            oFile.InputStream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch { }
            br.Close();
            ms.Close();
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe) return true;
            }
            return false;
        }

        public static bool IsSecureUploadPhoto(HttpPostedFile oFile)
        {
            bool isPhoto = false;
            string fileExtension = System.IO.Path.GetExtension(oFile.FileName).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }
            if (!isPhoto)
            {
                return true;
            }
            FileExtension[] fe = { FileExtension.BMP, FileExtension.GIF, FileExtension.JPG, FileExtension.PNG };

            if (IsAllowedExtension(oFile, fe))
                return true;
            else
                return false;
        }

        public static bool IsSecureUpfilePhoto(string photoFile)
        {
            bool isPhoto = false;
            string Img = "Yes";
            string fileExtension = System.IO.Path.GetExtension(photoFile).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }

            if (!isPhoto)
            {
                return true;
            }
            StreamReader sr = new StreamReader(photoFile, System.Text.Encoding.Default);
            string strContent = sr.ReadToEnd();
            sr.Close();
            string str = "request|<script|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
            foreach (string s in str.Split('|'))
            {
                if (strContent.ToLower().IndexOf(s) != -1)
                {
                    File.Delete(photoFile);
                    Img = "No";
                    break;
                }
            }
            return (Img == "Yes");
        }
    }
}