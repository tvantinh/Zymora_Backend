using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Core.Base;

namespace Zymora_BE.Contract.Repositories.Entities
{
  public class UserToken : BaseModel
  {
    [MaxLength(450)]
    public required string LoginProvider { get; set; }

    [MaxLength(128)]
    public required string Name { get; set; }
    public string? Value { get; set; }

    public required User User { get; set; }
  }
}