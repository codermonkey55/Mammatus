using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;

namespace Mammatus.Library.Smtp
{
    public class MessageSender
    {
        private readonly SmtpClient _mailClient;

        public MessageSender(string server, int port)
        {
            _mailClient = port > 0 ? new SmtpClient(server, port) : new SmtpClient(server);

            _mailClient.SendCompleted += SendComplete;
        }

        public void SendMessage(MailAddress toAddress,
                                MailAddress fromAddress,
                                string subject,
                                string msg,
                                List<Attachment> at,
                                MessageDetails state)
        {
            try
            {
                state.MMessage = new MailMessage(fromAddress, toAddress);
                if (at != null && at.Count > 0)
                {
                    foreach (Attachment a in at)
                    {
                        state.MMessage.Attachments.Add(a);
                    }
                }
                state.MMessage.Body = msg;
                state.MMessage.BodyEncoding = System.Text.Encoding.UTF8;
                state.MMessage.Subject = subject;
                state.MMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                _mailClient.SendAsync(state.MMessage, state);
            }
            catch (Exception e)
            {
                state.RaiseException(e);
            }
        }

        public void CancelMailSend()
        {
            _mailClient.SendAsyncCancel();
        }

        private void SendComplete(object sender, AsyncCompletedEventArgs e)
        {
            MessageDetails state = (MessageDetails)e.UserState;

            if (e.Cancelled)
            {
                state.RaiseException(new Exception("Send cancelled."));
            }

            if (e.Error != null)
            {
                state.RaiseException(new Exception("Sending mail failed: " + e.Error));
            }
            else
            {
                state.OperationComplete();
            }
        }
    }
}
