using Mammatus.Library.Mime;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace Mammatus.Library.Mail
{
    /// <summary>
    /// This class adds a few internet mail headers not already exposed by the
    /// System.Net.MailMessage.  It also provides support to encapsulate the
    /// nested mail attachments in the Children collection.
    /// </summary>
    public class MailMessageEx : System.Net.Mail.MailMessage
    {
        public const string EmailRegexPattern = "(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?";

        public long Octets { get; set; }

        /// <summary>
        /// Gets or sets the message number of the MailMessage on the POP3 server.
        /// </summary>
        /// <value>The message number.</value>
        public int MessageNumber { get; internal set; }

        private static readonly char[] AddressDelimiters = new char[] { ',', ';' };

        /// <summary>
        /// Gets the children MailMessage attachments.
        /// </summary>
        /// <value>The children MailMessage attachments.</value>
        public List<MailMessageEx> Children { get; }

        /// <summary>
        /// Gets the delivery date.
        /// </summary>
        /// <value>The delivery date.</value>
        public DateTime DeliveryDate
        {
            get
            {
                string date = GetHeader(MailHeaders.Date);
                if (string.IsNullOrEmpty(date))
                {
                    return DateTime.MinValue;
                }

                if ((date.IndexOf("(EST)") > 1) && (date.IndexOf("-") > 1))
                {
                    date = date.Substring(0, date.IndexOf("-"));
                }
                return Convert.ToDateTime(date);
            }
        }

        /// <summary>
        /// Gets the return address.
        /// </summary>
        /// <value>The return address.</value>
        public MailAddress ReturnAddress
        {
            get
            {
                string replyTo = GetHeader(MailHeaders.ReplyTo);
                if (string.IsNullOrEmpty(replyTo))
                {
                    return null;
                }

                return CreateMailAddress(replyTo);
            }
        }

        /// <summary>
        /// Gets the routing.
        /// </summary>
        /// <value>The routing.</value>
        public string Routing => GetHeader(MailHeaders.Received);

        /// <summary>
        /// Gets the message id.
        /// </summary>
        /// <value>The message id.</value>
        public string MessageId => GetHeader(MailHeaders.MessageId);

        public string ReplyToMessageId => GetHeader(MailHeaders.InReplyTo, true);

        /// <summary>
        /// Gets the MIME version.
        /// </summary>
        /// <value>The MIME version.</value>
        public string MimeVersion => GetHeader(MimeHeaders.MimeVersion);

        /// <summary>
        /// Gets the content id.
        /// </summary>
        /// <value>The content id.</value>
        public string ContentId => GetHeader(MimeHeaders.ContentId);

        /// <summary>
        /// Gets the content description.
        /// </summary>
        /// <value>The content description.</value>
        public string ContentDescription => GetHeader(MimeHeaders.ContentDescription);

        /// <summary>
        /// Gets the content disposition.
        /// </summary>
        /// <value>The content disposition.</value>
        public ContentDisposition ContentDisposition
        {
            get
            {
                string contentDisposition = GetHeader(MimeHeaders.ContentDisposition);
                if (string.IsNullOrEmpty(contentDisposition))
                {
                    return null;
                }

                return new ContentDisposition(contentDisposition);
            }
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public ContentType ContentType
        {
            get
            {
                string contentType = GetHeader(MimeHeaders.ContentType);
                if (string.IsNullOrEmpty(contentType))
                {
                    return null;
                }

                return MimeReader.GetContentType(contentType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailMessageEx"/> class.
        /// </summary>
        public MailMessageEx()
            : base()
        {
            Children = new List<MailMessageEx>();
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <returns></returns>
        private string GetHeader(string header)
        {
            return GetHeader(header, false);
        }

        private string GetHeader(string header, bool stripBrackets)
        {
            if (stripBrackets)
            {
                return MimeEntity.TrimBrackets(Headers[header]);
            }

            return Headers[header];
        }

        /// <summary>
        /// Creates the mail message from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static MailMessageEx CreateMailMessageFromEntity(MimeEntity entity)
        {
            MailMessageEx message = new MailMessageEx();
            string value;
            foreach (string key in entity.Headers.AllKeys)
            {
                value = entity.Headers[key];
                if (value.Equals(string.Empty))
                {
                    value = " ";
                }

                message.Headers.Add(key.ToLowerInvariant(), value);

                switch (key.ToLowerInvariant())
                {
                    case MailHeaders.Bcc:
                        MailMessageEx.PopulateAddressList(value, message.Bcc);
                        break;
                    case MailHeaders.Cc:
                        MailMessageEx.PopulateAddressList(value, message.CC);
                        break;
                    case MailHeaders.From:
                        message.From = MailMessageEx.CreateMailAddress(value);
                        break;
                    case MailHeaders.ReplyTo:
                        message.ReplyTo = MailMessageEx.CreateMailAddress(value);
                        break;
                    case MailHeaders.Subject:
                        message.Subject = value;
                        break;
                    case MailHeaders.To:
                        MailMessageEx.PopulateAddressList(value, message.To);
                        break;
                }
            }

            return message;
        }

        /// <summary>
        /// Creates the mail address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static MailAddress CreateMailAddress(string address)
        {
            try
            {
                return new MailAddress(address.Trim('\t'));
            }
            catch //(FormatException e)
            {
                //throw new Pop3Exception("Unable to create mail address from provided string: " + address, e);Mail Delivery System
                return new MailAddress(address + "@mail.error");
            }
        }

        /// <summary>
        /// Populates the address list.
        /// </summary>
        /// <param name="addressList">The address list.</param>
        /// <param name="recipients">The recipients.</param>
        public static void PopulateAddressList(string addressList, MailAddressCollection recipients)
        {
            foreach (MailAddress address in GetMailAddresses(addressList))
            {
                recipients.Add(address);
            }
        }

        /// <summary>
        /// Gets the mail addresses.
        /// </summary>
        /// <param name="addressList">The address list.</param>
        /// <returns></returns>
        private static IEnumerable<MailAddress> GetMailAddresses(string addressList)
        {
            Regex email = new Regex(EmailRegexPattern);

            foreach (Match match in email.Matches(addressList))
            {
                yield return CreateMailAddress(match.Value);
            }
        }
    }
}
