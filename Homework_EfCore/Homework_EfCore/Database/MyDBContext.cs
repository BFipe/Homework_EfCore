﻿using Homework_EfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework_EfCore.Database
{
    public class MyDBContext : DbContext
    {
        public MyDBContext()
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBooks> UserBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                const string sqlAddress = "Server=localhost;Database=Homework_EfCore;Trusted_Connection=true;Encrypt=false";
                optionsBuilder.UseSqlServer(sqlAddress);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserBookConfiguration());
        }
    }
}
