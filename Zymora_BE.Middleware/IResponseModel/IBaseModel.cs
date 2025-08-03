using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.IResponseModel
{
    public interface IBaseModel<T>
    {
        public int StatusCode { get; set; } // HTTP status code
        public bool Success { get; set; } // indicates if the operation was successful
        public string? Message { get; set; } // message providing additional information about the response
        public T? Data { get; set; } // generic data type for the response payload
        public IEnumerable<ValidationError>? Errors { get; set; } // object to hold any errors that may have occurred
        public string TraceId { get; set; } // unique identifier for tracing the request
        public DateTime Timestamp { get; set; } // timestamp of the response

    }
    public record ValidationError(string Field, string message);
}
