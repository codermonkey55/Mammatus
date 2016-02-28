using System;
using System.Linq;
using System.Collections.Generic;

namespace Mammatus.Operation.Results
{
    public interface IListResult<TItem> : IResult<IList<TItem>>, IEnumerable<TItem>
    {

    }
}