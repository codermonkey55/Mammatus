using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Mammatus.Library.Mail
{

    public class SmtpMail
    {
        public SmtpMail()
        { }

        private StreamReader _sr;
        private StreamWriter _sw;
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;

        private bool SendDataToServer(string str)
        {
            try
            {
                _sw.WriteLine(str);
                _sw.Flush();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        private string ReadDataFromServer()
        {
            string str = null;
            try
            {
                str = _sr.ReadLine();
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

        public ArrayList ReceiveMail(string uid, string pwd)
        {
            ArrayList emailMes = new ArrayList();
            string str;
            int index = uid.IndexOf('@');
            string pop3Server = "pop3." + uid.Substring(index + 1);
            _tcpClient = new TcpClient(pop3Server, 110);
            _networkStream = _tcpClient.GetStream();
            _sr = new StreamReader(_networkStream);
            _sw = new StreamWriter(_networkStream);

            if (ReadDataFromServer() == null) return emailMes;
            if (SendDataToServer("USER " + uid) == false) return emailMes;
            if (ReadDataFromServer() == null) return emailMes;
            if (SendDataToServer("PASS " + pwd) == false) return emailMes;
            if (ReadDataFromServer() == null) return emailMes;
            if (SendDataToServer("LIST") == false) return emailMes;
            if ((str = ReadDataFromServer()) == null) return emailMes;

            string[] splitString = str.Split(' ');
            int count = int.Parse(splitString[1]);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if ((str = ReadDataFromServer()) == null) return emailMes;
                    splitString = str.Split(' ');
                    emailMes.Add($"{splitString[0]}，{splitString[1]}");
                }
                return emailMes;
            }
            else
            {
                return emailMes;
            }
        }

        public string ReadEmail(string str)
        {
            string state = "";
            if (SendDataToServer("RETR " + str) == false)
                state = "Error";
            else
            {
                state = _sr.ReadToEnd();
            }
            return state;
        }

        public string DeleteEmail(string str)
        {
            string state;
            if (SendDataToServer("DELE " + str) == true)
            {
                state = "";
            }
            else
            {
                state = "Error";
            }
            return state;
        }

        public void CloseConnection()
        {
            SendDataToServer("QUIT");
            _sr.Close();
            _sw.Close();
            _networkStream.Close();
            _tcpClient.Close();
        }
    }

}