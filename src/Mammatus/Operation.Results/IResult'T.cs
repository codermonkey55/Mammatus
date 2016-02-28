using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mammatus.Operation.Results
{
    public interface IResult<T> : IResult
    {
        T Data { get; set; }
    }

}
