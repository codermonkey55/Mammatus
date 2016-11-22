using System;
using System.IO;
using System.Text;

namespace Mammatus.Library.FTP
{
    public class FTPOperater
    {
        public FTPClient Ftp { get; set; }

        public string Server { get; set; }

        public string User { get; set; }

        public string Pass { get; set; }

        public string FolderZJ { get; set; }

        public string FolderWX { get; set; }

        public string[] GetList(string strPath)
        {
            if (Ftp == null) Ftp = this.GetFtpClient();
            Ftp.Connect();
            Ftp.ChDir(strPath);
            return Ftp.Dir("*");
        }

        public bool GetFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (!Ftp.Connected)
                {
                    Ftp.Connect();
                    Ftp.ChDir(ftpFolder);
                }
                Ftp.Get(ftpFileName, localFolder, localFileName);

                return true;
            }
            catch
            {
                try
                {
                    Ftp.DisConnect();
                    Ftp = null;
                }
                catch { Ftp = null; }
                return false;
            }
        }

        public bool AddMSCFile(string ftpFolder, string ftpFileName, string localFolder, string localFileName, string BscInfo)
        {
            string sLine = "";
            string sResult = "";
            string path = "";
            path = path.Substring(0, path.LastIndexOf("\\"));
            try
            {
                FileStream fsFile = new FileStream(ftpFolder + "\\" + ftpFileName, FileMode.Open);
                FileStream fsFileWrite = new FileStream(localFolder + "\\" + localFileName, FileMode.Create);
                StreamReader sr = new StreamReader(fsFile);
                StreamWriter sw = new StreamWriter(fsFileWrite);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while (sr.Peek() > -1)
                {
                    sLine = sr.ReadToEnd();
                }
                string[] arStr = sLine.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arStr.Length - 1; i++)
                {
                    sResult += BscInfo + "," + arStr[i].Trim() + "\n";
                }
                sr.Close();
                byte[] connect = new UTF8Encoding(true).GetBytes(sResult);
                fsFileWrite.Write(connect, 0, connect.Length);
                fsFileWrite.Flush();
                sw.Close();
                fsFile.Close();
                fsFileWrite.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DelFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (!Ftp.Connected)
                {
                    Ftp.Connect();
                    Ftp.ChDir(ftpFolder);
                }
                Ftp.Delete(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool PutFile(string ftpFolder, string ftpFileName)
        {
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (!Ftp.Connected)
                {
                    Ftp.Connect();
                    Ftp.ChDir(ftpFolder);
                }
                Ftp.Put(ftpFileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetFileNoBinary(string ftpFolder, string ftpFileName, string localFolder, string localFileName)
        {
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (!Ftp.Connected)
                {
                    Ftp.Connect();
                    Ftp.ChDir(ftpFolder);
                }
                Ftp.GetNoBinary(ftpFileName, localFolder, localFileName);
                return true;
            }
            catch
            {
                try
                {
                    Ftp.DisConnect();
                    Ftp = null;
                }
                catch
                {
                    Ftp = null;
                }
                return false;
            }
        }

        public string GetFileInfo(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (Ftp.Connected) Ftp.DisConnect();
                Ftp.Connect();
                Ftp.ChDir(ftpFolder);
                strResult = Ftp.GetFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        public bool CanConnect()
        {
            if (Ftp == null) Ftp = this.GetFtpClient();
            try
            {
                Ftp.Connect();
                Ftp.DisConnect();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFileInfoConnected(string ftpFolder, string ftpFileName)
        {
            string strResult = "";
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (!Ftp.Connected)
                {
                    Ftp.Connect();
                    Ftp.ChDir(ftpFolder);
                }
                strResult = Ftp.GetFileInfo(ftpFileName);
                return strResult;
            }
            catch
            {
                return "";
            }
        }

        public string[] GetFileList(string ftpFolder, string strMask)
        {
            string[] strResult;
            try
            {
                if (Ftp == null) Ftp = this.GetFtpClient();
                if (!Ftp.Connected)
                {
                    Ftp.Connect();
                    Ftp.ChDir(ftpFolder);
                }
                strResult = Ftp.Dir(strMask);
                return strResult;
            }
            catch
            {
                return null;
            }
        }

        public FTPClient GetFtpClient()
        {
            FTPClient ft = new FTPClient();
            ft.RemoteHost = this.Server;
            ft.RemoteUser = this.User;
            ft.RemotePass = this.Pass;
            return ft;
        }
    }
}