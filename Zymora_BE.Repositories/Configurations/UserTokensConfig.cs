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
  public class UserTokensConfig : IEntityTypeConfiguration<UserToken>
  {
    public void Configure(EntityTypeBuilder<UserToken> e)
    {
      e.ToTable("user_tokens");
      e.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

      e.Property(t => t.UserId).HasMaxLength(450).IsRequired();
      e.Property(t => t.LoginProvider).HasMaxLength(128).IsRequired();
      e.Property(t => t.Name).HasMaxLength(128).IsRequired();

      e.HasOne(t => t.User)
       .WithMany(u => u.Tokens)
       .HasForeignKey(t => t.UserId)
       .OnDelete(DeleteBehavior.Cascade);
    }
  } 
}
