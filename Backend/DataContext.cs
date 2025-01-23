﻿using System;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend;

public class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<ConfigUser> ConfigUsers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Config> Configs { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionCategory> QuestionCategories { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<ConfigUserToken> ConfigUserTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<BackgroundImage> BackgroundImages { get; set; }
    public DbSet<Tasks> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            entityType.SetTableName(entityType.DisplayName().ToLower());

        modelBuilder.UseSerialColumns();
        base.OnModelCreating(modelBuilder);

        CreateBackgroundImage(modelBuilder);
        CreateConfigUser(modelBuilder);
        CreateRole(modelBuilder);
        CreateConfig(modelBuilder);
        CreateQuestion(modelBuilder);
        CreateQuestionCategory(modelBuilder);
        CreateAnswer(modelBuilder);
        CreateConfigUserToken(modelBuilder);
        CreateRefreshToken(modelBuilder);
        CreateSetting(modelBuilder);
        CreateTasks(modelBuilder);

        // Seed initial data
        modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "ADMIN" });
        modelBuilder.Entity<ConfigUser>().HasData(new ConfigUser
        {
            Id = 1,
            FirstName = "test",
            LastName = "test",
            Email = "test",
            Password = "$2a$11$TxzkGMQgywQjBxMq9YcOoO66hQODh5zJzIg4npGPDzfpcefvKORD2",
            RoleId = 1,
            RefreshToken = "refresh_token_value",
            TokenCreated = DateTime.UtcNow,
            TokenExpires = DateTime.UtcNow.AddDays(7)
        });
    }

    private void CreateTasks(ModelBuilder modelBuilder)
    {
        var tasks = modelBuilder.Entity<Tasks>();
        tasks.Property(t => t.Id).UseIdentityColumn();
        tasks.Property(t => t.Text).IsRequired();
        tasks.Property(t => t.Done).IsRequired();
    }

    private static void CreateBackgroundImage(ModelBuilder modelBuilder)
    {
        var backgroundImage = modelBuilder.Entity<BackgroundImage>();
        backgroundImage
            .Property(bi => bi.Id)
            .UseIdentityColumn();
        backgroundImage
            .Property(bi => bi.Base64Image)
            .IsRequired();
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
        question.Property(q => q.Id).UseIdentityColumn();
        question.Property(q => q.Text).IsRequired();
        question.HasMany(q => q.Categories).WithMany(c => c.Questions);
    }

    private static void CreateQuestionCategory(ModelBuilder modelBuilder)
    {
        var questionCategory = modelBuilder.Entity<QuestionCategory>();
        questionCategory.Property(c => c.Id).UseIdentityColumn();
        questionCategory.Property(c => c.Name).IsRequired();
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

    private static void CreateRefreshToken(ModelBuilder modelBuilder)
    {
        var refreshToken = modelBuilder.Entity<RefreshToken>();
        refreshToken
            .Property(rt => rt.Id)
            .UseIdentityColumn();
    }

    private static void CreateSetting(ModelBuilder modelBuilder)
    {
        var setting = modelBuilder.Entity<Setting>();
        setting.HasData(new Setting()
        {
            Id = 1,
            Name = "Rezep-1",
            BackgroundImage = "https://example.com/image.jpg",
            BackgroundImageId = 1,
            GreetingMessage = "Hello, how can I help you?",
            Language = "en-US",
            TalkingSpeed = 0.7,
            State = true,
            AiInUse = "ChatGPT"
        });
        setting
            .Property(s => s.Id)
            .UseIdentityColumn();
        setting
            .Property(s => s.Name)
            .IsRequired();
        setting
            .Property(s => s.BackgroundImage)
            .IsRequired();
        setting
            .Property(s => s.BackgroundImageId)
            .IsRequired();
        setting
            .Property(s => s.Language)
            .IsRequired();
        setting
            .Property(s => s.TalkingSpeed)
            .IsRequired();
        setting
            .Property(s => s.GreetingMessage)
            .IsRequired();
        setting
            .Property(s => s.State)
            .IsRequired();
        setting
            .Property(s => s.AiInUse)
            .IsRequired();
    }
    
}