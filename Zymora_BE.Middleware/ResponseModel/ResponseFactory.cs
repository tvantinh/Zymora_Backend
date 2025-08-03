using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Middleware.IResponseModel;

namespace Zymora_BE.Middleware.ResponseModel
{
    public static class ResponseFactory
    {
        public static BaseModel<T> ResponseSuccess<T>(int statusCode, bool success, string message, T data, string TraceId) => // static method to create a successful response
            new() { StatusCode = statusCode, Success = success, Message = message, Data = data, Timestamp = DateTime.UtcNow, TraceId = TraceId};
        public static BaseModel<T> ResponseError<T>(int statusCode, bool success, string message, IEnumerable<ValidationError> errors, string TraceID) => // static method to create an error response
            new() { StatusCode = statusCode, Success = success, Message = message, Errors = errors, Timestamp = DateTime.UtcNow, TraceId = TraceID};
    }
}