using System;
using System.IO;
using Mammatus.Interface.Contracts;
using Mammatus.Models;

namespace Mammatus.Helpers
{
    public class FileHandler
    {
        public const int BufferSize = 4096;

        public static bool WriteFile(IRaiseEvent bh, int t, FileInfo fileInfo, byte[] buffer, bool rename)
        {
            try
            {
                if (fileInfo.Exists)
                {
                    if (rename)
                    {
                        DateTime td = DateTime.Now;
                        int index = fileInfo.FullName.LastIndexOf('.');
                        string newName = fileInfo.FullName.Remove(index, fileInfo.FullName.Length - index);
                        newName = newName + td.ToString("MMddHHmmss") + ".txt";
                        fileInfo.CopyTo(newName);
                    }
                    else
                    {
                        fileInfo.Delete();
                    }
                }

                FileReadWriteState ws = new FileReadWriteState(bh, t, fileInfo);
                ws.MFileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, true);

                IAsyncResult asyncResult = ws.MFileStream.BeginWrite(
                        buffer, 0, buffer.Length,
                        new AsyncCallback(WriteFileComplete), ws);
            }
            catch (Exception e)
            {
                FileReadWriteState st = new FileReadWriteState(bh, t, fileInfo);
                st.RaiseException(e);
            }
            return true;
        }

        public static void ReadFile(IRaiseEvent bh, int t, FileInfo fileInfo)
        {
            try
            {
                if (fileInfo.Exists)
                {
                    FileReadWriteState ws = new FileReadWriteState(bh, t, fileInfo);
                    ws.MFileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);

                    IAsyncResult asyncResult = ws.MFileStream.BeginRead(
                                ws.MBuffer, 0, ws.MBuffer.Length,
                                new AsyncCallback(ReadFileComplete), ws);
                }
                else
                {
                    throw new Exception("File: " + fileInfo.FullName + " does not exist");
                }
            }
            catch (Exception e)
            {
                FileReadWriteState st = new FileReadWriteState(bh, t, fileInfo);
                st.RaiseException(e);
            }
        }

        private static void WriteFileComplete(IAsyncResult asyncResult)
        {
            try
            {
                FileReadWriteState tempState = (FileReadWriteState)asyncResult.AsyncState;
                FileStream fStream = tempState.MFileStream;
                fStream.EndWrite(asyncResult);
                fStream.Close();
                tempState.AllWriteComplete();
            }
            catch (Exception e)
            {
                FileReadWriteState tempState = (FileReadWriteState)asyncResult.AsyncState;
                tempState.RaiseException(e);
            }
        }

        private static void ReadFileComplete(IAsyncResult asyncResult)
        {
            try
            {
                FileReadWriteState fileDetail = (FileReadWriteState)asyncResult.AsyncState;
                int readCount = fileDetail.MFileStream.EndRead(asyncResult);
                //
                // Read 0 means we have reached the end of the stream
                //
                if (readCount == 0)
                {
                    fileDetail.AllBufferFillComplete();
                    fileDetail.MFileStream.Close();
                }
                else
                {
                    fileDetail.BufferFillComplete(readCount);
                    fileDetail.MFileStream.BeginRead(fileDetail.MBuffer, 0, fileDetail.MBuffer.Length,
                                                  new AsyncCallback(ReadFileComplete), fileDetail);
                }
            }
            catch (Exception e)
            {
                FileReadWriteState fileDetail = (FileReadWriteState)asyncResult.AsyncState;
                fileDetail.RaiseException(e);
            }
        }
    }
}
