using System;
using System.IO;

namespace Mammatus.Library.Mail
{
    /// <summary>
    /// This class represents the Pop3 DELE command.
    /// </summary>
    internal sealed class DeleCommand : Pop3Command<Pop3Response>
    {
        readonly int _messageId = int.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleCommand"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="messageId">The message id.</param>
        public DeleCommand(Stream stream, int messageId)
            : base(stream, false, Pop3State.Transaction)
        {
            if (messageId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(messageId));
            }
            _messageId = messageId;
        }

        /// <summary>
        /// Creates the DELE request message.
        /// </summary>
        /// <returns>
        /// The byte[] containing the DELE request message.
        /// </returns>
        protected override byte[] CreateRequestMessage()
        {
            return GetRequestMessage(string.Concat(Pop3Commands.Dele, _messageId.ToString(), Pop3Commands.Crlf));
        }
    }
}
