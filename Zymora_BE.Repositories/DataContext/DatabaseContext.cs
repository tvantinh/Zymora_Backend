using Zymora_BE.Contract.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Repositories.DataContext
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DatabaseContext()
        {
        }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);

          modelBuilder.Entity<User>(entity =>
          {
            entity.ToTable("User");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasMaxLength(450)
                  .IsRequired();

            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            entity.Property(e => e.PhoneNumber).HasMaxLength(50);

            entity.Property(e => e.EmailConfirmed).HasDefaultValue(false);
            entity.Property(e => e.PhoneNumberConfirmed).HasDefaultValue(false);
            entity.Property(e => e.TwoFactorEnabled).HasDefaultValue(false);
            entity.Property(e => e.LockoutEnabled).HasDefaultValue(false);
            entity.Property(e => e.AccessFailedCount).HasDefaultValue(0);

            entity.HasIndex(e => e.NormalizedUserName)
                  .HasDatabaseName("UserNameIndex")
                  .IsUnique()
                  .HasFilter("[NormalizedUserName] IS NOT NULL");

            entity.HasIndex(e => e.NormalizedEmail)
                  .HasDatabaseName("EmailIndex");
          });
        }
  }
}
