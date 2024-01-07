using Microsoft.EntityFrameworkCore;
using backend.Entities;

namespace backend;

public class DataContext : DbContext
{
    public DbSet<ConfigUser> ConfigUsers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Config> Configs { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<ConfigUserToken> ConfigUserTokens { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.SetTableName(entityType.DisplayName().ToLower());
        }

        modelBuilder.UseSerialColumns();
        base.OnModelCreating(modelBuilder);

        CreateConfigUser(modelBuilder);
        CreateRole(modelBuilder);
        CreateConfig(modelBuilder);
        CreateQuestion(modelBuilder);
        CreateAnswer(modelBuilder);
        CreateConfigUserToken(modelBuilder);
    }

    private static void CreateConfigUser(ModelBuilder modelBuilder)
    {
        var configUser = modelBuilder.Entity<ConfigUser>();
        configUser
            .Property(cu => cu.Id)
            .UseIdentityColumn();
        configUser
            .Property(cu => cu.FirstName)
            .IsRequired();
        configUser
            .Property(cu => cu.LastName)
            .IsRequired();
        configUser
            .Property(cu => cu.Email)
            .IsRequired();
        configUser
            .Property(cu => cu.Password)
            .IsRequired();
        configUser
            .Property(cu => cu.RoleId)
            .IsRequired();
        configUser
            .HasOne(cu => cu.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(cu => cu.RoleId);
    }

    private static void CreateRole(ModelBuilder modelBuilder)
    {
        var role = modelBuilder.Entity<Role>();
        role
            .Property(r => r.Id)
            .UseIdentityColumn();
        role
            .Property(r => r.Name)
            .IsRequired();
    }

    private static void CreateConfig(ModelBuilder modelBuilder)
    {
        var config = modelBuilder.Entity<Config>();
        config
            .Property(c => c.Id)
            .UseIdentityColumn();
        config
            .Property(c => c.Title)
            .IsRequired();
        config
            .Property(c => c.Value)
            .IsRequired();
    }

    private static void CreateQuestion(ModelBuilder modelBuilder)
    {
        var question = modelBuilder.Entity<Question>();
        question
            .Property(q => q.Id)
            .UseIdentityColumn();
        question
            .Property(q => q.Text)
            .IsRequired();
        question
            .HasMany(q => q.Answers);
    }

    private static void CreateAnswer(ModelBuilder modelBuilder)
    {
        var answer = modelBuilder.Entity<Answer>();
        answer
            .Property(a => a.Id)
            .UseIdentityColumn();
        answer
            .Property(a => a.Text)
            .IsRequired();
    }

    private static void CreateConfigUserToken(ModelBuilder modelBuilder)
    {
        var configUserToken = modelBuilder.Entity<ConfigUserToken>();
        configUserToken
            .Property(cut => cut.Id)
            .UseIdentityColumn();
        configUserToken
            .Property(cut => cut.Token)
            .IsRequired();
        configUserToken
            .Property(cut => cut.Email)
            .IsRequired();
        configUserToken
            .Property(cut => cut.CreatedAt)
            .IsRequired();
        configUserToken
            .Property(cut => cut.RoleId)
            .IsRequired();
        configUserToken
            .HasOne(cut => cut.Role);
    }
}