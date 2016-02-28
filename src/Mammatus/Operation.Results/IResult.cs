using System;

namespace Mammatus.Operation.Results
{
    public interface IResult
    {
        bool Success { get; set; }

        string ErrorCode { get; set; }

        string Message { get; set; }
    }
}
