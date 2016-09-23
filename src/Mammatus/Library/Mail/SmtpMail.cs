using System;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace Mammatus.Library.Mail_Xofly
{

    /// <summary>
    /// 操作服务器上邮件
    /// </summary>
    public class SmtpMail
    {
        public SmtpMail()
        { }

        #region 字段
        private StreamReader sr;
        private StreamWriter sw;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        #endregion

        #region 私有方法
        /// <summary>
        /// 向服务器发送信息
        /// </summary>
        private bool SendDataToServer(string str)
        {
            try
            {
                sw.WriteLine(str);
                sw.Flush();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        /// <summary>
        /// 从网络流中读取服务器回送的信息
        /// </summary>
        private string ReadDataFromServer()
        {
            string str = null;
            try
            {
                str = sr.ReadLine();
                if (str[0] == '-')
                {
                    str = null;
                }
            }
            catch (Exception err)
            {
                str = err.Message;
            }
            return str;
        }
        #endregion

        #region 获取邮件信息
        /// <summary>
        /// 获取邮件信息
        /// </summary>
        /// <param name="uid">邮箱账号</param>
        /// <param name="pwd">邮箱密码</param>
        /// <returns>邮件信息</returns>
        public ArrayList ReceiveMail(string uid, string pwd)
        {
            ArrayList EmailMes = new ArrayList();
            string str;
            int index = uid.IndexOf('@');
            string pop3Server = "pop3." + uid.Substring(index + 1);
            tcpClient = new TcpClient(pop3Server, 110);
            networkStream = tcpClient.GetStream();
            sr = new StreamReader(networkStream);
            sw = new StreamWriter(networkStream);

            if (ReadDataFromServer() == null) return EmailMes;
            if (SendDataToServer("USER " + uid) == false) return EmailMes;
            if (ReadDataFromServer() == null) return EmailMes;
            if (SendDataToServer("PASS " + pwd) == false) return EmailMes;
            if (ReadDataFromServer() == null) return EmailMes;
            if (SendDataToServer("LIST") == false) return EmailMes;
            if ((str = ReadDataFromServer()) == null) return EmailMes;

            string[] splitString = str.Split(' ');
            int count = int.Parse(splitString[1]);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if ((str = ReadDataFromServer()) == null) return EmailMes;
                    splitString = str.Split(' ');
                    EmailMes.Add(string.Format("第{0}封邮件，{1}字节", splitString[0], splitString[1]));
                }
                return EmailMes;
            }
            else
            {
                return EmailMes;
            }
        }
        #endregion

        #region 读取邮件内容
        /// <summary>
        /// 读取邮件内容
        /// </summary>
        /// <param name="mailMessage">第几封</param>
        /// <returns>内容</returns>
        public string ReadEmail(string str)
        {
            string state = "";
            if (SendDataToServer("RETR " + str) == false)
                state = "Error";
            else
            {
                state = sr.ReadToEnd();
            }
            return state;
        }
        #endregion

        #region 删除邮件
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="str">第几封</param>
        /// <returns>操作信息</returns>
        public string DeleteEmail(string str)
        {
            string state = "";
            if (SendDataToServer("DELE " + str) == true)
            {
                state = "成功删除";
            }
            else
            {
                state = "Error";
            }
            return state;
        }
        #endregion

        #region 关闭服务器连接
        /// <summary>
        /// 关闭服务器连接
        /// </summary>
        public void CloseConnection()
        {
            SendDataToServer("QUIT");
            sr.Close();
            sw.Close();
            networkStream.Close();
            tcpClient.Close();
        }
        #endregion
    }

}