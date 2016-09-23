using System;
using Mammatus.Helpers;
using Mammatus.Interface.Contracts;

namespace Mammatus.Models
{
    using System.IO;

    public class FileReadWriteState
    {
        private readonly int _mEventT;
        private readonly IRaiseEvent _mHandler;
        private readonly MemoryStream _mStreamBuffer = new MemoryStream();
        private Exception _mOperationException;
        public FileStream MFileStream;
        public FileInfo MFileInfo;

        /// <summary>
        /// The buffer is filled by the operating system
        /// during a read operation, but may also be filled
        /// manually, by ManuallyAssignBuffer
        /// unfortunatly it has to be public so the os can access it
        /// </summary>
        public byte[] MBuffer = new byte[FileHandler.BufferSize];

        public FileReadWriteState(IRaiseEvent bh, int t, FileInfo file)
        {
            _mHandler = bh;
            _mEventT = t;
            MFileInfo = file;
        }

        internal void RaiseException(Exception ex)
        {
            _mOperationException = ex;
            _mHandler.RaiseEvent(_mEventT, this);
        }

        public Exception GetException()
        {
            return _mOperationException;
        }

        public MemoryStream GetStreamBuffer()
        {
            return _mStreamBuffer;
        }

        /// <summary>
        /// Manually assigning the buffer,
        /// as opposed to a read operation
        /// </summary>
        /// <param name="buf"></param>
        public void ManuallyAssignBuffer(byte[] buf)
        {
            MBuffer = buf;
            BufferFillComplete(buf.Length);
            AllBufferFillComplete();
        }

        /// <summary>
        /// The next two functions are used
        /// when reading a file and the mBuffer
        /// is filler by the operating system
        /// </summary>
        /// <param name="bytesRead"></param>
        public void BufferFillComplete(int bytesRead)
        {
            _mStreamBuffer.Write(MBuffer, 0, bytesRead);
        }

        public void AllBufferFillComplete()
        {
            _mStreamBuffer.Seek(0, SeekOrigin.Begin);
            _mHandler.RaiseEvent(_mEventT, this);
        }

        /// <summary>
        /// Called when all data has been written to disk
        /// </summary>
        public void AllWriteComplete()
        {
            _mHandler.RaiseEvent(_mEventT, this);
        }
    }
}
