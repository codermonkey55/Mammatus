using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Data.Infrastructure
{
    using System;

     public interface ISessionResolver
     {
         Guid GetSessionKeyFor<T>();
    }
}
