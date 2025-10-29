using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Models
{
  public class PagedResponse<T>
  {
    [JsonPropertyName("items")]
    public IEnumerable<T> Items { get; set; }

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; }

    public PagedResponse(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
      Items = items;
      Pagination = new PaginationMetadata
      {
        CurrentPage = pageNumber,
        PageSize = pageSize,
        TotalCount = totalCount,
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
        HasPrevious = pageNumber > 1,
        HasNext = pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
      };
    }
  }
}
