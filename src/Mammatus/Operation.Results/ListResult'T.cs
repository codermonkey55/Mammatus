using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Mammatus.Operation.Results
{
    public class ListResult<TItem> : Result<IList<TItem>>, IListResult<TItem>
    {
        public IEnumerator<TItem> GetEnumerator()
        {
            if (base.Data == null)
                return System.Linq.Enumerable.Empty<TItem>().GetEnumerator();

            return this.Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}