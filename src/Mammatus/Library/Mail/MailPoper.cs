using System;
using System.Collections.Generic;
using Mammatus.Library.POP3;

namespace Mammatus.Library.Mail
{
    public class MailPoper
    {
        public static List<MailMessageEx> Receive()
        {
            PopSetting ps = PopConfig.Create().PopSetting;
            return Receive(ps.Server, ps.Port, ps.UseSsl, ps.UserName, ps.Password);
        }

        public static List<MailMessageEx> Receive(string hostname, int port, bool useSsl, string username, string password)
        {
            using (Pop3Client client = new Pop3Client(hostname, port, useSsl, username, password))
            {
                client.Trace += new Action<string>(Console.WriteLine);
                client.Authenticate();
                client.Stat();
                List<MailMessageEx> maillist = new List<MailMessageEx>();
                MailMessageEx message = null;
                foreach (Pop3ListItem item in client.List())
                {
                    message = client.RetrMailMessageEx(item);
                    if (message != null)
                    {
                        client.Dele(item);
                        maillist.Add(message);
                    }

                }
                client.Noop();
                client.Rset();
                client.Quit();
                return maillist;
            }
        }
    }


}
