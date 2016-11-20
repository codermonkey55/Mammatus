using System;
using System.IO;

namespace Mammatus.Library.Mail
{
    internal sealed class ConnectResponse : Pop3Response
    {
        public Stream NetworkStream { get; }

        public ConnectResponse(Pop3Response response, Stream networkStream)
            : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
        {
            if (networkStream == null)
            {
                throw new ArgumentNullException(nameof(networkStream));
            }
            NetworkStream = networkStream;
        }
    }
}
