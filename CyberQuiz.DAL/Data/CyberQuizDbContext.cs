using CyberQuiz.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Data;

public class CyberQuizDbContext : IdentityDbContext<AppUser>
{
    public CyberQuizDbContext(DbContextOptions<CyberQuizDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<UserResult> UserResults => Set<UserResult>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Category → SubCategory
        builder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(sc => sc.Category)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // SubCategory → Question
        builder.Entity<SubCategory>()
            .HasMany(sc => sc.Questions)
            .WithOne(q => q.SubCategory)
            .HasForeignKey(q => q.SubCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question → AnswerOption
        builder.Entity<Question>()
            .HasMany(q => q.AnswerOptions)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // AppUser → UserResult
        builder.Entity<AppUser>()
            .HasMany(u => u.Results)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question → UserResult
        builder.Entity<Question>()
            .HasMany(q => q.Results)
            .WithOne(r => r.Question)
            .HasForeignKey(r => r.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // AnswerOption → UserResult
        builder.Entity<AnswerOption>()
            .HasMany(a => a.UserResults)
            .WithOne(r => r.AnswerOption)
            .HasForeignKey(r => r.AnswerOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}