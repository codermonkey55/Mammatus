using System.ComponentModel;

namespace Mammatus.Reactive.Notifications
{
    public class NotificationBase : DisposableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
