using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;

namespace Mammatus.Library.Mail
{
    public class SmtpServerHelper
    {
        public SmtpServerHelper()
        {
            SmtpCodeAdd();
        }

        ~SmtpServerHelper()
        {
            _networkStream.Close();
            _tcpClient.Close();
        }

        private readonly string _crlf = "\r\n";

        private TcpClient _tcpClient;

        private NetworkStream _networkStream;

        private string _logs = "";

        private readonly Hashtable _errCodeHt = new Hashtable();

        private readonly Hashtable _rightCodeHt = new Hashtable();

        public string ErrMsg { set; get; }

        private string Base64Encode(string str)
        {
            var barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        private string Base64Decode(string str)
        {
            byte[] barray;
            barray = Convert.FromBase64String(str);
            return Encoding.Default.GetString(barray);
        }

        private string GetStream(string filePath)
        {
            System.IO.FileStream fileStr = new System.IO.FileStream(filePath, System.IO.FileMode.Open);
            byte[] by = new byte[System.Convert.ToInt32(fileStr.Length)];
            fileStr.Read(by, 0, by.Length);
            fileStr.Close();
            return (System.Convert.ToBase64String(by));
        }

        private void SmtpCodeAdd()
        {
            _errCodeHt.Add("421", "，");
            _errCodeHt.Add("432", "");
            _errCodeHt.Add("450", "，（，）");
            _errCodeHt.Add("451", "；");
            _errCodeHt.Add("452", "，");
            _errCodeHt.Add("454", "");
            _errCodeHt.Add("500", "");
            _errCodeHt.Add("501", "");
            _errCodeHt.Add("502", "");
            _errCodeHt.Add("503", "");
            _errCodeHt.Add("504", "");
            _errCodeHt.Add("530", "");
            _errCodeHt.Add("534", "");
            _errCodeHt.Add("538", "");
            _errCodeHt.Add("550", "，（，，）");
            _errCodeHt.Add("551", "，<forward-path>");
            _errCodeHt.Add("552", "，");
            _errCodeHt.Add("553", "，（）");
            _errCodeHt.Add("554", "");

            _rightCodeHt.Add("220", "");
            _rightCodeHt.Add("221", "");
            _rightCodeHt.Add("235", "");
            _rightCodeHt.Add("250", "");
            _rightCodeHt.Add("251", "，<forward-path>");
            _rightCodeHt.Add("334", "");
            _rightCodeHt.Add("354", "，<CRLF>.<CRLF>");
        }

        private bool SendCommand(string str)
        {
            byte[] writeBuffer;
            if (str == null || str.Trim() == String.Empty)
            {
                return true;
            }
            _logs += str;
            writeBuffer = Encoding.Default.GetBytes(str);
            try
            {
                _networkStream.Write(writeBuffer, 0, writeBuffer.Length);
            }
            catch
            {
                ErrMsg = "";
                return false;
            }
            return true;
        }

        private string RecvResponse()
        {
            int streamSize;
            string returnvalue = String.Empty;
            byte[] readBuffer = new byte[1024];
            try
            {
                streamSize = _networkStream.Read(readBuffer, 0, readBuffer.Length);
            }
            catch
            {
                ErrMsg = "";
                return "false";
            }

            if (streamSize == 0)
            {
                return returnvalue;
            }
            else
            {
                returnvalue = Encoding.Default.GetString(readBuffer).Substring(0, streamSize);
                _logs += returnvalue + this._crlf;
                return returnvalue;
            }
        }

        private bool Dialog(string str, string errstr)
        {
            if (str == null || str.Trim() == string.Empty)
            {
                return true;
            }
            if (SendCommand(str))
            {
                string rr = RecvResponse();
                if (rr == "false")
                {
                    return false;
                }

                string rrCode = rr.Substring(0, 3);
                if (_rightCodeHt[rrCode] != null)
                {
                    return true;
                }
                else
                {
                    if (_errCodeHt[rrCode] != null)
                    {
                        ErrMsg += (rrCode + _errCodeHt[rrCode].ToString());
                        ErrMsg += _crlf;
                    }
                    else
                    {
                        ErrMsg += rr;
                    }
                    ErrMsg += errstr;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool Dialog(string[] str, string errstr)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!Dialog(str[i], ""))
                {
                    ErrMsg += _crlf;
                    ErrMsg += errstr;
                    return false;
                }
            }
            return true;
        }

        private bool Connect(string smtpServer, int port)
        {
            try
            {
                _tcpClient = new TcpClient(smtpServer, port);
            }
            catch (Exception e)
            {
                ErrMsg = e.ToString();
                return false;
            }
            _networkStream = _tcpClient.GetStream();

            if (_rightCodeHt[RecvResponse().Substring(0, 3)] == null)
            {
                ErrMsg = "";
                return false;
            }
            return true;
        }

        private string GetPriorityString(MailPriority mailPriority)
        {
            string priority = "Normal";
            if (mailPriority == MailPriority.Low)
            {
                priority = "Low";
            }
            else if (mailPriority == MailPriority.High)
            {
                priority = "High";
            }
            return priority;
        }

        private bool SendEmail(string smtpServer, int port, bool eSmtp, string username, string password, MailMessage mailMessage)
        {
            if (Connect(smtpServer, port) == false) return false;

            string priority = GetPriorityString(mailMessage.Priority);

            bool html = (mailMessage.BodyFormat == MailFormat.Html);

            string[] sendBuffer;
            string sendBufferstr;

            if (eSmtp)
            {
                sendBuffer = new String[4];
                sendBuffer[0] = "EHLO " + smtpServer + _crlf;
                sendBuffer[1] = "AUTH LOGIN" + _crlf;
                sendBuffer[2] = Base64Encode(username) + _crlf;
                sendBuffer[3] = Base64Encode(password) + _crlf;
                if (!Dialog(sendBuffer, "，。")) return false;
            }
            else
            {
                sendBufferstr = "HELO " + smtpServer + _crlf;
                if (!Dialog(sendBufferstr, "")) return false;
            }

            sendBufferstr = "MAIL FROM:<" + username + ">" + _crlf;
            if (!Dialog(sendBufferstr, "，")) return false;

            sendBuffer = new string[mailMessage.Recipients.Count];
            for (int i = 0; i < mailMessage.Recipients.Count; i++)
            {
                sendBuffer[i] = "RCPT TO:<" + (string)mailMessage.Recipients[i] + ">" + _crlf;
            }
            if (!Dialog(sendBuffer, "")) return false;

            sendBufferstr = "DATA" + _crlf;
            if (!Dialog(sendBufferstr, "")) return false;

            sendBufferstr = "From:" + mailMessage.FromName + "<" + mailMessage.From + ">" + _crlf;

            if (mailMessage.Recipients.Count == 0)
            {
                return false;
            }
            else
            {
                sendBufferstr += "To:=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode((string)mailMessage.Recipients[0]) + "?=" + "<" + (string)mailMessage.Recipients[0] + ">" + _crlf;
            }
            sendBufferstr += (string.IsNullOrEmpty(mailMessage.Subject) ? "Subject:" : ((mailMessage.Charset == "") ? ("Subject:" + mailMessage.Subject) : ("Subject:" + "=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode(mailMessage.Subject) + "?="))) + _crlf;
            sendBufferstr += "X-Priority:" + priority + _crlf;
            sendBufferstr += "X-MSMail-Priority:" + priority + _crlf;
            sendBufferstr += "Importance:" + priority + _crlf;
            sendBufferstr += "X-Mailer: Lion.Web.Mail.SmtpMail Pubclass [cn]" + _crlf;
            sendBufferstr += "MIME-Version: 1.0" + _crlf;
            if (mailMessage.Attachments.Count != 0)
            {
                sendBufferstr += "Content-Type: multipart/mixed;" + _crlf;
                sendBufferstr += " boundary=\"=====" + (html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====\"" + _crlf + _crlf;
            }
            if (html)
            {
                if (mailMessage.Attachments.Count == 0)
                {
                    sendBufferstr += "Content-Type: multipart/alternative;" + _crlf;
                    sendBufferstr += " boundary=\"=====003_Dragon520636771063_=====\"" + _crlf + _crlf;
                    sendBufferstr += "This is a multi-part message in MIME format." + _crlf + _crlf;
                }
                else
                {
                    sendBufferstr += "This is a multi-part message in MIME format." + _crlf + _crlf;
                    sendBufferstr += "--=====001_Dragon520636771063_=====" + _crlf;
                    sendBufferstr += "Content-Type: multipart/alternative;" + _crlf;
                    sendBufferstr += " boundary=\"=====003_Dragon520636771063_=====\"" + _crlf + _crlf;
                }
                sendBufferstr += "--=====003_Dragon520636771063_=====" + _crlf;
                sendBufferstr += "Content-Type: text/plain;" + _crlf;
                sendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" + mailMessage.Charset.ToLower() + "\"")) + _crlf;
                sendBufferstr += "Content-Transfer-Encoding: base64" + _crlf + _crlf;
                sendBufferstr += Base64Encode("，") + _crlf + _crlf;

                sendBufferstr += "--=====003_Dragon520636771063_=====" + _crlf;

                sendBufferstr += "Content-Type: text/html;" + _crlf;
                sendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" + mailMessage.Charset.ToLower() + "\"")) + _crlf;
                sendBufferstr += "Content-Transfer-Encoding: base64" + _crlf + _crlf;
                sendBufferstr += Base64Encode(mailMessage.Body) + _crlf + _crlf;
                sendBufferstr += "--=====003_Dragon520636771063_=====--" + _crlf;
            }
            else
            {
                if (mailMessage.Attachments.Count != 0)
                {
                    sendBufferstr += "--=====001_Dragon303406132050_=====" + _crlf;
                }
                sendBufferstr += "Content-Type: text/plain;" + _crlf;
                sendBufferstr += ((mailMessage.Charset == "") ? (" charset=\"iso-8859-1\"") : (" charset=\"" + mailMessage.Charset.ToLower() + "\"")) + _crlf;
                sendBufferstr += "Content-Transfer-Encoding: base64" + _crlf + _crlf;
                sendBufferstr += Base64Encode(mailMessage.Body) + _crlf;
            }
            if (mailMessage.Attachments.Count != 0)
            {
                for (int i = 0; i < mailMessage.Attachments.Count; i++)
                {
                    string filepath = (string)mailMessage.Attachments[i];
                    sendBufferstr += "--=====" + (html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====" + _crlf;
                    sendBufferstr += "Content-Type: text/plain;" + _crlf;
                    sendBufferstr += " name=\"=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + _crlf;
                    sendBufferstr += "Content-Transfer-Encoding: base64" + _crlf;
                    sendBufferstr += "Content-Disposition: attachment;" + _crlf;
                    sendBufferstr += " filename=\"=?" + mailMessage.Charset.ToUpper() + "?B?" + Base64Encode(filepath.Substring(filepath.LastIndexOf("\\") + 1)) + "?=\"" + _crlf + _crlf;
                    sendBufferstr += GetStream(filepath) + _crlf + _crlf;
                }
                sendBufferstr += "--=====" + (html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====--" + _crlf + _crlf;
            }
            sendBufferstr += _crlf + "." + _crlf;
            if (!Dialog(sendBufferstr, "")) return false;

            sendBufferstr = "QUIT" + _crlf;
            if (!Dialog(sendBufferstr, "")) return false;

            _networkStream.Close();
            _tcpClient.Close();
            return true;
        }

        public bool SendEmail(string smtpServer, int port, MailMessage mailMessage)
        {
            return SendEmail(smtpServer, port, false, "", "", mailMessage);
        }

        public bool SendEmail(string smtpServer, int port, string username, string password, MailMessage mailMessage)
        {
            return SendEmail(smtpServer, port, true, username, password, mailMessage);
        }
    }
}