using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using San3a.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using San3a.Core.Enums;

namespace San3a.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Craftsman> Craftsmen { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Job> JobPosts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .OwnsMany(u => u.RefreshTokens, rt =>
                {
                    rt.WithOwner().HasForeignKey("UserId");
                    rt.Property(r => r.Token).IsRequired();
                    rt.HasKey("UserId", "Token");
                });

            builder.Entity<Admin>()
                .HasOne(a => a.AppUser)
                .WithOne()
                .HasForeignKey<Admin>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer>()
                .HasOne(c => c.AppUser)
                .WithOne()
                .HasForeignKey<Customer>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Craftsman>()
                .HasOne(c => c.AppUser)
                .WithOne()
                .HasForeignKey<Craftsman>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Craftsman>()
                .HasOne(c => c.Service)
                .WithMany(s => s.Craftsmen)
                .HasForeignKey(c => c.ServiceId);

            builder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.WrittenReviews)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Reviewee)
                .WithMany(u => u.ReceivedReviews)
                .HasForeignKey(r => r.RevieweeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.Customer)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CustomerId);

            builder.Entity<Job>()
                .HasOne(j => j.serviceType)
                .WithMany(s => s.Jobs)
                .HasForeignKey(j => j.ServiceId);

            builder.Entity<Job>()
                .HasOne(j => j.AcceptedWorker)
                .WithMany(c => c.AcceptedJobs)
                .HasForeignKey(j => j.AcceptedCraftsmanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Offer>()
                .HasOne(o => o.Job)
                .WithMany(j => j.Offers)
                .HasForeignKey(o => o.JobId);

            builder.Entity<Offer>()
                .HasOne(o => o.Worker)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CraftsmanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}