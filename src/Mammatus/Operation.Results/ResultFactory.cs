using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Operation.Results
{
    public sealed class ResultFactory
    {
        private ResultFactory()
        {

        }

        public static IResult Create()
        {
            return new Result();
        }

        public static IResult<T> Create<T>()
        {
            return new Result<T>();
        }

        public static IResult Create(bool success, string message = null, string errorCode = null)
        {
            return new Result { Success = success, Message = message, ErrorCode = errorCode };
        }

        public static IResult<T> Create<T>(T data, bool success, string message = null, string errorCode = null)
        {
            return new Result<T> { Success = success, Message = message, ErrorCode = errorCode, Data = data };
        }

        public static IResult Success(string message = null, string errorCode = null)
        {
            return new Result { Success = true, Message = message, ErrorCode = errorCode };
        }

        public static IResult<T> Success<T>(T data, string message = null, string errorCode = null)
        {
            return new Result<T> { Success = true, Message = message, ErrorCode = errorCode, Data = data };
        }

        public static IResult Failed(string message = null, string errorCode = null)
        {
            return new Result { Success = false, Message = message, ErrorCode = errorCode };
        }

        public static IResult<T> Failed<T>(string message = null, string errorCode = null)
        {
            return new Result<T> { Success = false, Message = message, ErrorCode = errorCode };
        }



        public static IListResult<T> CreateList<T>()
        {
            return new ListResult<T>();
        }

        public static IListResult<T> CreateList<T>(bool success, string message = null, string errorCode = null)
        {
            return new ListResult<T> { Success = success, Message = message, ErrorCode = errorCode };
        }

        public static IListResult<T> CreateList<T>(IList<T> data, bool success, string message = null, string errorCode = null)
        {
            return new ListResult<T> { Success = success, Message = message, ErrorCode = errorCode };
        }

        public static IListResult<T> SuccessList<T>(IList<T> data, string message = null, string errorCode = null)
        {
            return new ListResult<T> { Success = true, Message = message, ErrorCode = errorCode, Data = data };
        }

        public static IListResult<T> FailedList<T>(string message = null, string errorCode = null)
        {
            return new ListResult<T> { Success = false, Message = message, ErrorCode = errorCode };
        }

    }
}
