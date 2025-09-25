using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Zymora_BE.Contract.Repositories.Entities;
using Zymora_BE.Repositories.Configurations;

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
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> User_tokens { get; set; }
        public DbSet<UserRoles> User_roles { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserClaims> User_claims { get; set; }
        public DbSet<RolesClaims> Roles_claims { get; set; }
        public DbSet<UserLogins> User_logins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);

          modelBuilder.ApplyConfiguration(new UserConfig());
          modelBuilder.ApplyConfiguration(new RolesConfig());
          modelBuilder.ApplyConfiguration(new UserRolesConfig());
          modelBuilder.ApplyConfiguration(new UserClaimsConfig());
          modelBuilder.ApplyConfiguration(new RolesClaimsConfig());
          modelBuilder.ApplyConfiguration(new UserLoginsConfig());
          modelBuilder.ApplyConfiguration(new UserTokensConfig());
        }
  }
}
