using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Models
{
  public class ApiResponse<T>
  {
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public Dictionary<string, List<string>> Errors { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; set; }

    public ApiResponse()
    {
      Timestamp = DateTime.UtcNow;
      Errors = new Dictionary<string, List<string>>();
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
    {
      return new ApiResponse<T>
      {
        Success = true,
        Message = message,
        Data = data
      };
    }

    public static ApiResponse<T> ErrorResponse(string message, Dictionary<string, List<string>>? errors = null)
    {
      return new ApiResponse<T>
      {
        Success = false,
        Message = message,
        Errors = errors ?? new Dictionary<string, List<string>>()
      };
    }
  }
}
