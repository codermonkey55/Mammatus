using System;
using System.IO;
using System.Net;
using Mammatus.Interface.Contracts;

namespace Mammatus.Models
{
    public class FtpState
    {
        public FtpWebRequest MRequest;
        public string MStatus;
        public MemoryStream MData;

        private Exception _mOperationException;
        private readonly IRaiseEvent _mHandler;
        private readonly int _eventId;

        public FtpState(IRaiseEvent h, int e, FileInfo fname)
        {
            _mHandler = h;
            _eventId = e;
            MData = new MemoryStream();
            FileName = fname;
        }
        public FileInfo FileName { get; }

        public Exception GetException()
        {
            return _mOperationException;
        }
        internal void RaiseException(Exception ex)
        {
            _mOperationException = ex;
            _mHandler.RaiseEvent(_eventId, this);
        }
        internal void OperationComplete()
        {
            _mOperationException = null;
            _mHandler.RaiseEvent(_eventId, this);
        }
    }

}