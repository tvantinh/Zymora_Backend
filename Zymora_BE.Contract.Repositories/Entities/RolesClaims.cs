using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Core.Base;

namespace Zymora_BE.Contract.Repositories.Entities
{
  public class RolesClaims : BaseModel
  {
    [MaxLength(256)]
    public string? ClaimType { get; set; }
    [MaxLength(256)]
    public string? ClaimValue { get; set; }
    public required string RoleId { get; set; }
    public required Roles Role { get; set; }
  }
}
