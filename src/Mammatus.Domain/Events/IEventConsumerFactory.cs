using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mammatus.Domain.Events;

namespace Mammatus.Domain.Events
{
    public interface IEventConsumerFactory
    {
        IEnumerable<IEventConsumer<T>> GetAllEventConsumers<T>();
    }
}
