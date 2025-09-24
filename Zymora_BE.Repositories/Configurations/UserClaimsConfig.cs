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
  public class UserClaimsConfig : IEntityTypeConfiguration<UserClaims>
  {
    public void Configure(EntityTypeBuilder<UserClaims> e)
    {
      e.ToTable("user_claims");
      e.HasKey(x => x.Id);
      e.Property(x => x.ClaimType).HasMaxLength(256);
      e.Property(x => x.UserId).HasMaxLength(450).IsRequired();
      e.HasIndex(x => x.UserId);

      e.HasOne(x => x.User)
       .WithMany(u => u.Claims)
       .HasForeignKey(x => x.UserId)
       .IsRequired()
       .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
