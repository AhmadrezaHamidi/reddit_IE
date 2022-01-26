using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Reddit
{
    public class RedditContext : DbContext
    {
        public static string connectionString = "Data Source=.; database=Reddit; integrated security=true";

        public RedditContext() : base(GetOptions(connectionString)) { }

        public DbSet<Post> Posts { get; set; }
        public string DbPath { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configures one-to-many relationship
            // modelBuilder.Entity<Post>()
            //     .HasOne<Post>(b => b.CurrentAuthor)
            //     .WithMany(a => a.Users)
            //     .HasForeignKey(b => b.Author);
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}