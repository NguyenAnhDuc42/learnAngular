using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserLike>().HasKey(k => new {k.SourceUserId, k.TagetUserId});

        modelBuilder.Entity<UserLike>().HasOne(s => s.SourceUser)
                    .WithMany(s => s.LikedUsers)
                    .HasForeignKey(s => s.SourceUserId)
                    .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<UserLike>().HasOne(s => s.TagetUser)
                    .WithMany(s => s.LikedByUser)
                    .HasForeignKey(s => s.TagetUserId)
                    .OnDelete(DeleteBehavior.NoAction);  
    }

}
