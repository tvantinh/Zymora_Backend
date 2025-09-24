using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Core.Base;

namespace Zymora_BE.Contract.Repositories.Entities
{
  public class Roles : BaseModel
  {
    [MaxLength(256)]
    public string? Name { get; set; }
    [MaxLength(256)]
    public string? NormalizedName { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public ICollection<UserRoles>? UserRoles { get; set; }
    public ICollection<RolesClaims>? RoleClaims { get; set; }
  }
}
