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
  public class UserRolesConfig : IEntityTypeConfiguration<UserRoles>
  {
    public void Configure(EntityTypeBuilder<UserRoles> e)
    {
      e.ToTable("user_roles");

      e.HasKey(ur => new { ur.UserId, ur.RoleId });

      e.Property(ur => ur.UserId).HasMaxLength(450).IsRequired();
      e.Property(ur => ur.RoleId).HasMaxLength(450).IsRequired();

      e.HasOne(ur => ur.User)
       .WithMany(u => u.UserRoles)
       .HasForeignKey(ur => ur.UserId)
       .OnDelete(DeleteBehavior.Cascade);

      e.HasOne(ur => ur.Role)
       .WithMany(r => r.UserRoles)
       .HasForeignKey(ur => ur.RoleId)
       .OnDelete(DeleteBehavior.NoAction);
    }
  }
}
