using System;
using System.ServiceModel;

namespace Mammatus.ServiceModel.Core
{
    public class ServiceClient<TChannel> : ServiceClientBase<TChannel>, IDisposable where TChannel : class
    {
        private bool _disposed;

        public ServiceClient()
        {
        }

        public ServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public TChannel Client
        {
            get { return Channel; }
        }

        public void Close(bool dispose)
        {
            try
            {
                base.Close();
            }
            finally
            {
                ((IDisposable)this).Dispose();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    try
                    {
                        if (State != CommunicationState.Closed)
                            base.Close();
                    }
                    catch (CommunicationException)
                    {
                        base.Abort();
                    }
                    catch (TimeoutException)
                    {
                        base.Abort();
                    }
                    catch
                    {
                        base.Abort();
                        throw;
                    }

                    this._disposed = true;
                }
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
