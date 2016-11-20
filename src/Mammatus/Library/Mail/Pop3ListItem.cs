using System;

namespace Mammatus.Library.Mail
{
    /// <summary>
    /// This class represents an item returned from the
    /// Pop3 LIST command.
    /// </summary>
    public class Pop3ListItem
    {
        /// <summary>
        /// Gets or sets the message number.
        /// </summary>
        /// <value>The message number.</value>
        public int MessageId { get; set; }

        /// <summary>
        /// Gets or sets the octets.
        /// </summary>
        /// <value>The octets.</value>
        public long Octets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3ListItem"/> class.
        /// </summary>
        /// <param name="messageNumber">The message number.</param>
        /// <param name="octets">The octets.</param>
        public Pop3ListItem(int messageNumber, long octets)
        {
            if (messageNumber < 0)
            {
                throw new ArgumentOutOfRangeException("messageNumber");
            }

            if (octets < 1)
            {
                throw new ArgumentOutOfRangeException("octets");
            }

            MessageId = messageNumber;
            Octets = octets;
        }
    }
}
