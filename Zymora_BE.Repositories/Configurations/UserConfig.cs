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
  public class UserConfig : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> e)
    {
      e.ToTable("user");
      e.HasKey(x => x.Id);

      e.Property(x => x.Id).HasMaxLength(450).IsRequired();
      e.Property(x => x.UserName).HasMaxLength(256);
      e.Property(x => x.NormalizedUserName).HasMaxLength(256);
      e.Property(x => x.Email).HasMaxLength(256);
      e.Property(x => x.NormalizedEmail).HasMaxLength(256);
      e.Property(x => x.PhoneNumber).HasMaxLength(50);
      e.Property(x => x.LockoutEnd).HasColumnType("datetimeoffset(7)");

      // default constraints (DB-level)
      e.Property(x => x.EmailConfirmed).HasDefaultValue(false);
      e.Property(x => x.PhoneNumberConfirmed).HasDefaultValue(false);
      e.Property(x => x.TwoFactorEnabled).HasDefaultValue(false);
      e.Property(x => x.LockoutEnabled).HasDefaultValue(false);
      e.Property(x => x.AccessFailedCount).HasDefaultValue(0);

      // filtered unique index (chuẩn của Identity)
      e.HasIndex(x => x.NormalizedUserName)
       .HasDatabaseName("UserNameIndex")
       .IsUnique()
       .HasFilter("[NormalizedUserName] IS NOT NULL");

      e.HasIndex(x => x.NormalizedEmail)
       .HasDatabaseName("EmailIndex");

      //e.HasMany(x => x.UserRoles)
      // .WithOne(ur => ur.User)
      // .HasForeignKey(ur => ur.UserId)
      // .OnDelete(DeleteBehavior.Cascade);

      //e.HasMany(x => x.Claims)
      // .WithOne(c => c.User)
      // .HasForeignKey(c => c.UserId)
      // .OnDelete(DeleteBehavior.Cascade);

      //e.HasMany(x => x.Logins)
      // .WithOne(l => l.User)
      // .HasForeignKey(l => l.UserId)
      // .OnDelete(DeleteBehavior.Cascade);

      //e.HasMany(x => x.Tokens)
      // .WithOne(t => t.User)
      // .HasForeignKey(t => t.UserId)
      // .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
