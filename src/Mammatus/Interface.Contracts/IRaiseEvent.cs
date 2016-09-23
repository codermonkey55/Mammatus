namespace Mammatus.Interface.Contracts
{
    /// <summary>
    /// Used by asyncronous classes such as FTP_Handler or FileHandler etc
    /// Intend to be used with the BasHandler from the Dispatcher library,
    /// but not absolutly necessary.
    /// The intention of this interface is to de-couple the CommonLib
    /// from the DispatcherLib
    ///
    /// </summary>
    public interface IRaiseEvent
    {
        void RaiseEvent(int evt);
        void RaiseEvent(int evt, object parameter);
    }
}
