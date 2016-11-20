using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Mammatus.Library.Mail
{

    public class MailMessage
    {
        public MailMessage()
        {
            Recipients = new ArrayList();
            Attachments = new MailAttachments();
            BodyFormat = MailFormat.Html;
            Priority = MailPriority.Normal;
            Charset = "GB2312";
        }

        public string Charset { get; set; } = "GB2312";

        public int MaxRecipientNum { get; set; } = 30;

        public string From { get; set; }

        public string FromName { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }

        public MailAttachments Attachments { get; set; }

        public MailPriority Priority { get; set; }

        public IList Recipients { get; }

        public MailFormat BodyFormat { set; get; }

        public void AddRecipients(string recipient)
        {
            if (Recipients.Count < MaxRecipientNum)
            {
                Recipients.Add(recipient);
            }
        }

        public void AddRecipients(params string[] recipient)
        {
            if (recipient == null)
            {
                throw (new ArgumentException("."));
            }
            else
            {
                for (int i = 0; i < recipient.Length; i++)
                {
                    AddRecipients(recipient[i]);
                }
            }
        }
    }

}