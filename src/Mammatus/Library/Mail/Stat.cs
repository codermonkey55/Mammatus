﻿using System;

namespace Mammatus.Library.Mail
{
    /// <summary>
    /// This class represents the results from the execution
    /// of a pop3 STAT command.
    /// </summary>
    public sealed class Stat
    {
        /// <summary>
        /// Gets or sets the message count.
        /// </summary>
        /// <value>The message count.</value>
        public int MessageCount { get; set; }

        /// <summary>
        /// Gets or sets the octets.
        /// </summary>
        /// <value>The octets.</value>
        public long Octets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stat"/> class.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <param name="octets">The octets.</param>
        public Stat(int messageCount, long octets)
        {
            if (messageCount < 0)
            {
                throw new ArgumentOutOfRangeException("messageCount");
            }

            if (octets < 0)
            {
                throw new ArgumentOutOfRangeException("octets");
            }
            MessageCount = messageCount;
            Octets = octets;
        }
    }
}
