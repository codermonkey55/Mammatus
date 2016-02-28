using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;

namespace Mammatus.ServiceModel.Helpers
{
    public class ServiceProxyHelper<TProxy, TChannel> : IDisposable
        where TProxy : ClientBase<TChannel>, new()
        where TChannel : class
    {
        private TProxy _proxy;

        public ServiceProxyHelper(TProxy proxy)
        {
            this._proxy = proxy;
        }

        public TProxy Proxy
        {
            get
            {
                if (this._proxy != null)
                {
                    return this._proxy;
                }
                else
                {
                    throw new ObjectDisposedException("ServiceProxyHelper");
                }
            }
        }

        public void Dispose()
        {
            try
            {
                if (this._proxy != null)
                {
                    if (this._proxy.State != CommunicationState.Faulted)
                    {
                        this._proxy.Close();
                    }
                    else
                    {
                        this._proxy.Abort();
                    }
                }
            }
            catch (CommunicationException)
            {
                this._proxy.Abort();
            }
            catch (TimeoutException)
            {
                this._proxy.Abort();
            }
            catch (Exception)
            {
                this._proxy.Abort();
                throw;
            }
            finally
            {
                this._proxy = null;
            }
        }
    }
}