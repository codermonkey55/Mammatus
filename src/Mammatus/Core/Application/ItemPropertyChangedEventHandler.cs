using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mammatus.Core.Application
{
    public delegate void ItemPropertyChangedEventHandler<T>(object sender, ItemPropertyChangedEventArgs<T> e)
            where T : INotifyPropertyChanged;
}
