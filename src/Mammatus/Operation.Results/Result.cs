using System.Runtime.Serialization;

namespace Mammatus.Operation.Results
{
    public class Result : IResult
    {
        public bool Success { get; set; }

        public string ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
