using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mammatus.Operation.Results
{
    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; set; }
    }

}
