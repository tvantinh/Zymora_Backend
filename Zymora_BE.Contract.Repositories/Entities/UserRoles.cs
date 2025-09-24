using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Core.Base;

namespace Zymora_BE.Contract.Repositories.Entities
{
  public class UserRoles : BaseModel
  {
    public required User User { get; set; }
    public required Roles Role { get; set; }
  }
}
