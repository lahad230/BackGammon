using Microsoft.EntityFrameworkCore;
using SharedAssets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.DAL.DataContext
{
    public class BgDataContext : DbContext
    {
        public BgDataContext(DbContextOptions<BgDataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex("UserName").IsUnique();
            modelBuilder.Entity<User>().Property("UserName").IsRequired();
            modelBuilder.Entity<User>().Property("Password").IsRequired();
        }
    }
}
