using System.Net;
using System.Net.Mail;
using System.Text;

namespace Mammatus.Library.Mail
{
    public class MailSender
    {
        protected MailSender()
        {

        }

        public static void Send(string server, string sender,
                                string recipient, string subject,
                                string body, bool isBodyHtml, Encoding encoding,
                                bool isAuthentication, params string[] files)
        {
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient(server);
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(sender, recipient);
            message.IsBodyHtml = isBodyHtml;

            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;

            message.Subject = subject;
            message.Body = body;

            message.Attachments.Clear();
            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    Attachment attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }

            if (isAuthentication == true)
            {
                smtpClient.Credentials = new NetworkCredential(SmtpConfig.Create().SmtpSetting.User,
                    SmtpConfig.Create().SmtpSetting.Password);
            }
            smtpClient.Send(message);


        }

        public static void Send(string recipient, string subject, string body)
        {
            Send(SmtpConfig.Create().SmtpSetting.Server, SmtpConfig.Create().SmtpSetting.Sender, recipient, subject, body, true, Encoding.Default, true, null);
        }

        public static void Send(string Recipient, string Sender, string Subject, string Body)
        {
            Send(SmtpConfig.Create().SmtpSetting.Server, Sender, Recipient, Subject, Body, true, Encoding.UTF8, true, null);
        }
    }
}
