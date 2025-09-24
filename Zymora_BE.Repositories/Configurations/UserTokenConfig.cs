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
  public class UserTokenConfig : IEntityTypeConfiguration<UserToken>
  {
    public void Configure(EntityTypeBuilder<UserToken> e)
    {
      e.ToTable("user_tokens");
      e.HasKey(x => x.Id);
      e.Property(x => x.Id).HasMaxLength(450).IsRequired();
      e.Property(x => x.LoginProvider).HasMaxLength(450).IsRequired();
      e.Property(x => x.Name).HasMaxLength(128).IsRequired();
      e.Property(x => x.Value);
      e.HasOne(x => x.User)
       .WithMany()
       .HasForeignKey("UserId")
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);
    }
  } 
}
