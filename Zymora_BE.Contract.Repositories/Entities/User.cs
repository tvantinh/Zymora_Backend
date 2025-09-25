using Zymora_BE.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Contract.Repositories.Entities
{
    public class User : BaseModel
    {

      [MaxLength(256)]
      public string? UserName { get; set; }

      [MaxLength(256)]
      public string? NormalizedUserName { get; set; }

      [MaxLength(256)]
      [EmailAddress]
      public string? Email { get; set; }

      [MaxLength(256)]
      public string? NormalizedEmail { get; set; }

      public bool EmailConfirmed { get; set; }             

      public string? PasswordHash { get; set; }          
      public string? SecurityStamp { get; set; } 
      public string? ConcurrencyStamp { get; set; }

      [MaxLength(50)]
      public string? PhoneNumber { get; set; }

      public bool PhoneNumberConfirmed { get; set; }       
      public bool TwoFactorEnabled { get; set; }       

      public DateTimeOffset? LockoutEnd { get; set; }      
      public bool LockoutEnabled { get; set; }

      public int AccessFailedCount { get; set; }
      public virtual ICollection<UserToken>? Tokens { get; set; }
      public virtual ICollection<UserClaims>? Claims { get; set; }
      public virtual ICollection<UserLogins>? Logins { get; set; }
      public virtual ICollection<UserRoles>? UserRoles { get; set; }
     
  }
}
