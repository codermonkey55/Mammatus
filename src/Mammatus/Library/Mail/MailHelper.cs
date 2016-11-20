using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Mammatus.Library.Mail
{
    public class MailHelper
    {
        protected MailHelper()
        {

        }

        public static string GetEMailLoginUrl(string email)
        {
            if ((email == string.Empty) || (email.IndexOf("@", StringComparison.Ordinal) <= 0))
            {
                return string.Empty;
            }
            int index = email.IndexOf("@", StringComparison.Ordinal);
            email = "http://mail." + email.Substring(index + 1);
            return email;
        }

        public static string SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string hostIp)
        {
            string str = "";
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                {
                    IsBodyHtml = false,
                    Subject = mailSubjct,
                    Body = mailBody,
                    From = new MailAddress(mailFrom)
                };
                for (int i = 0; i < mailAddress.Count; i++)
                {
                    message.To.Add(mailAddress[i]);
                }
                new System.Net.Mail.SmtpClient { UseDefaultCredentials = false, DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis, Host = hostIp, Port = (char)0x19 }.Send(message);
            }
            catch (Exception exception)
            {
                str = exception.Message;
            }
            return str;
        }

        public static bool SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string hostIp, string username, string password)
        {
            bool flag;
            string str = SendMail(mailSubjct, mailBody, mailFrom, mailAddress, hostIp, 0x19, username, password, false, string.Empty, out flag);
            return flag;
        }

        public static string SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string hostIp, string filename, string username, string password, bool ssl)
        {
            string str = "";
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                {
                    IsBodyHtml = false,
                    Subject = mailSubjct,
                    Body = mailBody,

                    From = new MailAddress(mailFrom)
                };
                foreach (string address in mailAddress)
                {
                    message.To.Add(address);
                }
                if (System.IO.File.Exists(filename))
                {
                    message.Attachments.Add(new Attachment(filename));
                }
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    EnableSsl = ssl,
                    UseDefaultCredentials = false
                };
                NetworkCredential credential = new NetworkCredential(username, password);
                client.Credentials = credential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = hostIp;
                client.Port = 0x19;
                client.Send(message);
            }
            catch (Exception exception)
            {
                str = exception.Message;
            }
            return str;
        }

        public static string SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string hostIp, int port, string username, string password, bool ssl, string replyTo, out bool sendOk)
        {
            sendOk = true;
            string str = "";
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage
                {
                    IsBodyHtml = false,
                    Subject = mailSubjct,
                    Body = mailBody,
                    From = new MailAddress(mailFrom)
                };
                if (replyTo != string.Empty)
                {
                    MailAddress address = new MailAddress(replyTo);
                    message.ReplyTo = address;
                }
                Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                foreach (string address in mailAddress)
                {
                    if (regex.IsMatch(address))
                    {
                        message.To.Add(address);
                    }
                }
                if (message.To.Count == 0)
                {
                    return string.Empty;
                }
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    EnableSsl = ssl,
                    UseDefaultCredentials = false
                };
                NetworkCredential credential = new NetworkCredential(username, password);
                client.Credentials = credential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = hostIp;
                client.Port = port;
                client.Send(message);
            }
            catch (Exception exception)
            {
                str = exception.Message;
                sendOk = false;
            }
            return str;
        }
    }
}
