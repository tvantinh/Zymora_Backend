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
  public class UserLoginsConfig : IEntityTypeConfiguration<UserLogins>
  {
    public void Configure(EntityTypeBuilder<UserLogins> e)
    {
      e.ToTable("user_logins");
      e.HasKey(l => new { l.LoginProvider, l.ProviderKey });

      e.Property(l => l.LoginProvider).HasMaxLength(128).IsRequired();
      e.Property(l => l.ProviderKey).HasMaxLength(128).IsRequired();
      e.Property(l => l.ProviderDisplayName).HasMaxLength(256);

      e.Property(l => l.UserId).HasMaxLength(450).IsRequired();
      e.HasIndex(l => l.UserId);

      e.HasOne(l => l.User)
       .WithMany(u => u.Logins)
       .HasForeignKey(l => l.UserId)
       .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
