using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Middleware.IResponseModel;

namespace Zymora_BE.Middleware.ResponseModel
{
    public class BaseModel <T> : IBaseModel<T>
    {
        public int StatusCode { get; set; } // HTTP status code
        public bool Success { get; set; } // indicates if the operation was successful
        public string? Message { get; set; } // message providing additional information about the response
        public T? Data { get; set; } // generic data type for the response payload
        public IEnumerable<ValidationError>? Errors { get; set; } // collection to hold validation errors
        public required string TraceId { get; set; } // unique identifier for tracing the request
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;// timestamp of the response
    }
}
