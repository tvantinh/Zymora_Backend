using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Models
{
  public class PaginationMetadata
  {
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious { get; set; }

    [JsonPropertyName("hasNext")]
    public bool HasNext { get; set; }
  }
}
