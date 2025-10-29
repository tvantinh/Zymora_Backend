using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Models
{
  public class FilterParams
  {
    [JsonPropertyName("searchTerm")]
    public string? SearchTerm { get; set; }

    [JsonPropertyName("filters")]
    public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
  }
}
