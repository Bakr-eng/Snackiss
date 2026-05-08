using Microsoft.EntityFrameworkCore;
using Snackis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Infrastructure.Data
{
    public class SnackisDbContext : DbContext
    {
        public SnackisDbContext(DbContextOptions<SnackisDbContext> options)
           : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }


        public DbSet<Post> Posts { get; set; }

        public DbSet<Coment> Coments { get; set; }

        public DbSet<Report> Reports { get; set; }
    }
}
