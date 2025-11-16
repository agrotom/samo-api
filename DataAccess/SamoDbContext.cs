using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Samo.Models;
using Samo.Utility;

namespace Samo.DataAccess;

public class SamoDbContext(DbContextOptions<SamoDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Insight> Insights => Set<Insight>();
    public DbSet<Focus> Focuses => Set<Focus>();
    public DbSet<Thanks> Thanks => Set<Thanks>();
    public DbSet<Problems> Problems => Set<Problems>();
    public DbSet<Models.Results> Results => Set<Models.Results>();
    public DbSet<TomorrowMainTasks> TomorrowMainTasks => Set<TomorrowMainTasks>();
    public DbSet<Tracker> Trackers => Set<Tracker>();
    public DbSet<Rite> Rites => Set<Rite>();
    public DbSet<SelfHappiness> SelfHappiness => Set<SelfHappiness>();
    public DbSet<WorkHappiness> WorkHappiness => Set<WorkHappiness>();
    public DbSet<WeekHabit> WeekHabits => Set<WeekHabit>();
    public DbSet<TodayDeed> TodayDeeds => Set<TodayDeed>();
    public DbSet<Objective> Objectives => Set<Objective>();
    public DbSet<ObjectiveGoal> ObjectiveGoals => Set<ObjectiveGoal>();
    public DbSet<Mission> Missions => Set<Mission>();
    
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

        builder.HasDbFunction(() => DbFuncs.ToLocalTime(default, default)).HasName("get_start_of_day").HasSchema("public");
    }
}