using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Core.Base;

namespace Zymora_BE.Contract.Repositories.Entities
{
  public class UserLogins : BaseModel
  {
    [MaxLength(128)]
    public required string LoginProvider { get; set; }
    [MaxLength(128)]
    public required string ProviderKey { get; set; }
    [MaxLength(256)]
    public string? ProviderDisplayName { get; set; }
    public required string UserId { get; set; }
    public virtual required User User { get; set; }
  }
}
