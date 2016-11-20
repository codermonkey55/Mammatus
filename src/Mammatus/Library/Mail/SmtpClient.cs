using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Mammatus.Library.Mail
{

    public class SmtpClient
    {
        public SmtpClient()
        { }

        public SmtpClient(string smtpServer)
        {
            SmtpServer = smtpServer;
        }

        public string ErrMsg { get; private set; }

        public string SmtpServer { set; get; }

        public bool Send(MailMessage mailMessage, string username, string password)
        {
            SmtpServerHelper helper = new SmtpServerHelper();
            if (helper.SendEmail(SmtpServer, 25, username, password, mailMessage))
                return true;
            else
            {
                ErrMsg = helper.ErrMsg;
                return false;
            }
        }
    }

}