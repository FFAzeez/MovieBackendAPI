using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieBackendAPI.Domain.Models;
using System;
using System.Collections.Generic;
namespace MovieBackendAPI.Infrastructure.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config)
        : base(options)
        {
            _config = config;
        }

        public AppDbContext(DbContextOptions options, DbContextOptionsBuilder optionsbuilder) : base(options)
        {
            optionsbuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("MovieBackendAPI"));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("MovieBackendAPI"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.UpdatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChanges();
        }

        public virtual DbSet<Movies> Movies { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
    }
}
