using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Contract.Repositories.Entities;

namespace Zymora_BE.Repositories.Configurations
{
  public class RolesClaimsConfig : IEntityTypeConfiguration<RolesClaims>
  {
    public void Configure(EntityTypeBuilder<RolesClaims> e)
    {
      e.ToTable("roles_claims");
      e.HasKey(x => x.Id); // int identity

      e.Property(x => x.ClaimType).HasMaxLength(256);
      e.Property(x => x.ClaimValue).HasMaxLength(256); // ERD: nvarchar(256)

      e.Property(x => x.RoleId).HasMaxLength(450).IsRequired();
      e.HasIndex(x => x.RoleId);

      e.HasOne(x => x.Role)
       .WithMany(r => r.RoleClaims)
       .HasForeignKey(x => x.RoleId)
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
