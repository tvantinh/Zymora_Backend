using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Models
{
  public class PaginationParams
  {
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize
    {
      get => _pageSize;
      set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    [JsonPropertyName("sortBy")]
    public string SortBy { get; set; } = "Id";  

    [JsonPropertyName("sortOrder")]
    public string SortOrder { get; set; } = "asc"; // asc or desc
  }
}
