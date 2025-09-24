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
  public class RolesConfig : IEntityTypeConfiguration<Roles>
  {
    public void Configure(EntityTypeBuilder<Roles> e)
    {
      e.ToTable("roles");
      e.HasKey(x => x.Id);

      e.Property(x => x.Id).HasMaxLength(450).IsRequired();
      e.Property(x => x.Name).HasMaxLength(256);
      e.Property(x => x.NormalizedName).HasMaxLength(256);

      e.HasIndex(x => x.NormalizedName)
       .HasDatabaseName("RoleNameIndex")
       .IsUnique()
       .HasFilter("[NormalizedName] IS NOT NULL");

      // 1 - n (Role ↔ RoleClaims)
      e.HasMany(r => r.RoleClaims)
       .WithOne(rc => rc.Role)
       .HasForeignKey(rc => rc.RoleId)
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);

      // 1 - n (Role ↔ UserRoles)
      // Tránh multiple cascade paths: để NoAction/Restrict ở phía Role
      e.HasMany(r => r.UserRoles)
       .WithOne(ur => ur.Role)
       .HasForeignKey(ur => ur.RoleId)
       .IsRequired()
       .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
