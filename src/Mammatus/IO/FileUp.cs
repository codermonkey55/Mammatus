using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Mammatus.IO
{
    public class FileUp
    {
        public byte[] GetBinaryFile(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream Fsm = null;
                try
                {
                    Fsm = File.OpenRead(filename);
                    return ConvertStreamToByteBuffer(Fsm);
                }
                catch
                {
                    return new byte[0];
                }
                finally
                {
                    Fsm?.Close();
                }
            }
            else
            {
                return new byte[0];
            }
        }

        public byte[] ConvertStreamToByteBuffer(Stream theStream)
        {
            MemoryStream tempStream = new MemoryStream();
            try
            {
                int bi;
                while ((bi = theStream.ReadByte()) != -1)
                {
                    tempStream.WriteByte(((byte)bi));
                }
                return tempStream.ToArray();
            }
            catch
            {
                return new byte[0];
            }
            finally
            {
                tempStream.Close();
            }
        }

        public string FileSc(FileUpload PosPhotoUpload, string saveFileName, string imagePath)
        {
            string state = "";
            if (PosPhotoUpload.HasFile)
            {
                if (PosPhotoUpload.PostedFile.ContentLength / 1024 < 10240)
                {
                    string MimeType = PosPhotoUpload.PostedFile.ContentType;
                    if (String.Equals(MimeType, "image/gif") || String.Equals(MimeType, "image/pjpeg"))
                    {
                        PosPhotoUpload.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(imagePath));
                    }
                    else
                    {
                        state = "";
                    }
                }
                else
                {
                    state = "";
                }
            }
            else
            {
                state = "";
            }
            return state;
        }

        public void SaveFile(byte[] binData, string fileName, string fileType)
        {
            FileStream fileStream = null;
            MemoryStream m = new MemoryStream(binData);
            try
            {
                string savePath = HttpContext.Current.Server.MapPath("~/File/");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string File = savePath + fileName + fileType;
                fileStream = new FileStream(File, FileMode.Create);
                m.WriteTo(fileStream);
            }
            finally
            {
                m.Close();
                fileStream?.Close();
            }
        }
    }
}