using System;
using System.Net;
using Mammatus.Models;

namespace Mammatus.Helpers
{
    using System.IO;

    public class FtpHandler
    {
        //----------------------------------------------------------------------------------------------
        private static void SetupRequest(Uri target, FtpState state, string fileName, string user, string passwrd)
        {
            if (target.Scheme != Uri.UriSchemeFtp)
            {
                throw new Exception("UploadRequest: Target uri was not ftp");
            }
            state.MRequest = (FtpWebRequest)WebRequest.Create(target + @"/" + fileName);
            state.MRequest.Proxy = null;
            //
            // This example uses anonymous logon.
            // The request is anonymous by default; the credential does not have to be specified.
            // The example specifies the credential only to
            // control how actions are logged on the server.
            //
            if (!string.IsNullOrEmpty(user))
            {
                state.MRequest.Credentials = new NetworkCredential(user, passwrd); //"anonymous", "janeDoe@contoso.com");
            }
        }
        //----------------------------------------------------------------------------------------------
        /// <summary>
        /// Make a FTP request to get a file from the server.
        /// It will raise an event on the BaseHandler, when done
        /// </summary>
        /// <param name="target"> url that is the name of the file being uploaded to the server</param>
        /// <param name="state">holds all the required data as supplied by the caller</param>
        /// <param name="user">username used to connect to the server</param>
        /// <param name="passwrd">username used to connect to the server</param>
        public static void DownloadRequest(Uri target, FtpState state, string user, string passwrd)
        {
            try
            {
                SetupRequest(target, state, state.FileName.Name, user, passwrd);
                state.MRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                //
                // Store the request in the object that we pass into the
                // asynchronous operations.
                // Asynchronously get the stream for the file contents.
                //
                state.MRequest.BeginGetResponse(EndDownloadResponseCallback,
                    state);


                //.BeginGetRequestStream(
                //    new AsyncCallback(EndDownloadGetStreamCallback),
                //    state
                //);
            }
            catch (Exception e)
            {
                state.RaiseException(e);
            }
        }
        //----------------------------------------------------------------------------------------------
        /// <summary>
        /// Make a FTP request to put a file onto the server.
        /// It will raise an event on the BaseHandler, when done
        /// </summary>
        /// <param name="target"> url that is the name of the file being uploaded to the server</param>
        /// <param name="state">holds all the required data as supplied by the caller</param>
        /// <param name="user">username used to connect to the server</param>
        /// <param name="passwrd">username used to connect to the server</param>
        public static void UploadRequest(Uri target, FtpState state, string user, string passwrd)
        {
            try
            {
                if (!state.FileName.Exists)
                {
                    throw new Exception("FTP_Handler::UploadRequest file: " + state.FileName.FullName
                        + " does not exist");
                }
                SetupRequest(target, state, state.FileName.Name, user, passwrd);
                state.MRequest.Method = WebRequestMethods.Ftp.UploadFile;
                //
                // Store the request in the object that we pass into the
                // asynchronous operations.
                // Asynchronously get the stream for the file contents.
                //
                state.MRequest.BeginGetRequestStream(EndUploadGetStreamCallback,
                    state
                );
            }
            catch (Exception e)
            {
                state.RaiseException(e);
            }
        }
        //----------------------------------------------------------------------------------------------
        private static void EndUploadGetStreamCallback(IAsyncResult asyncResult)
        {
            FtpState state = (FtpState)asyncResult.AsyncState;

            // End the asynchronous call to get the request stream.
            try
            {
                var requestStream = state.MRequest.EndGetRequestStream(asyncResult);

                // Copy the file contents to the request stream.
                const int bufferLength = 2048;

                byte[] buffer = new byte[bufferLength];

                //int count = 0;
                int readBytes;

                FileStream stream = state.FileName.OpenRead();

                do
                {
                    readBytes = stream.Read(buffer, 0, bufferLength);

                    requestStream.Write(buffer, 0, readBytes);
                    //count += readBytes;
                }
                while (readBytes != 0);

                // IMPORTANT: Close the request stream before sending the request.
                requestStream.Close();

                // Asynchronously get the response to the upload request.
                state.MRequest.BeginGetResponse(EndUploadResponseCallback, state);
            }
            // Return exceptions to the main application thread.
            catch (Exception e)
            {
                state.RaiseException(e);
            }
        }
        //----------------------------------------------------------------------------------------------
        // The EndGetResponseCallback method
        // completes a call to BeginGetResponse.
        private static void EndUploadResponseCallback(IAsyncResult asyncResult)
        {
            FtpState state = (FtpState)asyncResult.AsyncState;
            try
            {
                var response = (FtpWebResponse)state.MRequest.EndGetResponse(asyncResult);

                response.Close();

                state.MStatus = response.StatusDescription;

                state.OperationComplete();
            }
            // Return exceptions to the main application thread.
            catch (Exception e)
            {
                state.RaiseException(e);
            }
        }
        //----------------------------------------------------------------------------------------------
        // The EndGetResponseCallback method
        // completes a call to BeginGetResponse.
        //
        private static void EndDownloadResponseCallback(IAsyncResult asyncResult)
        {
            FtpState state = (FtpState)asyncResult.AsyncState;
            try
            {
                var response = (FtpWebResponse)state.MRequest.EndGetResponse(asyncResult);

                state.MStatus = response.StatusDescription;

                const int bufferLength = 2048;

                byte[] buffer = new byte[bufferLength];

                int readBytes = 0;

                do
                {
                    var responseStream = response.GetResponseStream();

                    if (responseStream != null)
                        readBytes = responseStream.Read(buffer, 0, bufferLength);

                    state.MData.Write(buffer, 0, readBytes);
                }
                while (readBytes > 0);

                response.Close();

                state.OperationComplete();
            }
            // Return exceptions to the main application thread.
            catch (Exception e)
            {
                state.RaiseException(e);
            }
        }
    }
}
