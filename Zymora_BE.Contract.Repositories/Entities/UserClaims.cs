using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Core.Base;

namespace Zymora_BE.Contract.Repositories.Entities
{
  public class UserClaims : BaseModel
  {
    [MaxLength(256)]
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
    public required string UserId { get; set; }
    public virtual required User User { get; set; }
  }
}
