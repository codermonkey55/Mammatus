﻿using System;
using System.IO;

namespace Mammatus.Library.POP3
{
    internal sealed class ConnectResponse : Pop3Response
    {
        private Stream _networkStream;
        public Stream NetworkStream
        {
            get
            {
                return _networkStream;
            }
        }

        public ConnectResponse(Pop3Response response, Stream networkStream)
            : base(response.ResponseContents, response.HostMessage, response.StatusIndicator)
        {
            if (networkStream == null)
            {
                throw new ArgumentNullException("networkStream");
            }
            _networkStream = networkStream;
        }
    }
}
