using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Samo.Models;

namespace Samo.DataAccess;

public class SamoDbContext(DbContextOptions<SamoDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Insight> Insights => Set<Insight>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity => entity.ToTable(name: "users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("user_roles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("user_claims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("user_logins"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("user_role_claims"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("user_tokens"));
        builder.Entity<Insight>(entity => entity.ToTable("insights"));
    }
}