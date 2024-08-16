using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ALSETDotnetTest.Models
{
    public class MainDBContext : DbContext //uses the entity framework to modelate the database 
    { 
        public MainDBContext(DbContextOptions<MainDBContext> options) //constructor accepts DbContextoptions and allos ef to create the database
            : base(options)
        {
        }
        //Query transalator from db context to sql server
        public DbSet<Researcher> Researchers { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1:N (1:N, 1:1 -> 1:N) 
            modelBuilder.Entity<Researcher>()
                .HasMany(r => r.Journals)
                .WithOne(j => j.Researcher)
                .HasForeignKey(j => j.ResearcherId);

            // N:M (1:N, N:1 -> N:M)
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Subscriber)
                .WithMany(r => r.Subscriptions)
                .HasForeignKey(s => s.SubscriberId)
                .OnDelete(DeleteBehavior.Restrict);
            //unique reference
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.SubscribedTo)
                .WithMany()
                .HasForeignKey(s => s.SubscribedToId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
