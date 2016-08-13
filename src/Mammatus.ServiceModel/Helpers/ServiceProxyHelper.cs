using System;
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
            _proxy = proxy;
        }

        public TProxy Proxy
        {
            get
            {
                if (_proxy != null)
                {
                    return _proxy;
                }
                throw new ObjectDisposedException("ServiceProxyHelper");
            }
        }

        public void Dispose()
        {
            try
            {
                if (_proxy != null)
                {
                    if (_proxy.State != CommunicationState.Faulted)
                    {
                        _proxy.Close();
                    }
                    else
                    {
                        _proxy.Abort();
                    }
                }
            }
            catch (CommunicationException)
            {
                _proxy.Abort();
            }
            catch (TimeoutException)
            {
                _proxy.Abort();
            }
            catch (Exception)
            {
                _proxy.Abort();
                throw;
            }
            finally
            {
                _proxy = null;
            }
        }
    }
}