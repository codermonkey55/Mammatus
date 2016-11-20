using System;

namespace Mammatus.Library.Mail
{
    /// <summary>
    /// This class represents a RETR response message resulting
    /// from a Pop3 RETR command execution against a Pop3 Server.
    /// </summary>
    internal sealed class RetrResponse : Pop3Response
    {
        /// <summary>
        /// Gets the message lines.
        /// </summary>
        /// <value>The Pop3 message lines.</value>
        public string[] MessageLines { get; }

        public long Octets { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrResponse"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="messageLines">The message lines.</param>
        public RetrResponse(Pop3Response response, string[] messageLines)
            : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
        {
            if (messageLines == null)
            {
                throw new ArgumentNullException("messageLines");
            }
            string[] values = response.HostMessage.Split(' ');
            if (values.Length == 2)
            {
                Octets = Convert.ToInt64(values[1]);
            }

            MessageLines = messageLines;
        }
    }
}
