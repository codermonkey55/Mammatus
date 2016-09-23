using System;
using System.Net.Mail;
using Mammatus.Interface.Contracts;

namespace Mammatus.Library.Smtp
{
    public class MessageDetails
    {
        private Exception _mOperationException;
        private readonly IRaiseEvent _mHandler;
        private readonly int _mEventId;
        public MailMessage MMessage;
        public object MArguments;

        public MessageDetails(IRaiseEvent h, int eventId)
        {
            _mHandler = h;
            _mEventId = eventId;
        }

        public Exception GetException()
        {
            return _mOperationException;
        }

        internal void RaiseException(Exception ex)
        {
            _mOperationException = ex;
            _mHandler.RaiseEvent(_mEventId, this);
        }

        internal void OperationComplete()
        {
            _mOperationException = null;
            _mHandler.RaiseEvent(_mEventId, this);
        }
    }
}