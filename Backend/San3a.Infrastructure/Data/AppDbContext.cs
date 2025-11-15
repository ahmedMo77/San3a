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
     public DbSet<Customer> Customers { get; set; }
     public DbSet<Worker> Workers { get; set; }
     public DbSet< JobPost >JobPosts { get; set; }
     public DbSet<Review> Reviews { get; set; }
     public DbSet<ServiceType> Services { get; set; }
     public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Worker>(entity =>
            {
                entity.ToTable("Workers");

                entity.HasKey(w => w.Id);

                entity.HasOne(w => w.User)
               .WithOne()
               .HasForeignKey<Worker>(w => w.UserId)
               .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(w => w.ServiceType)
                    .WithMany(w =>w.workers)
                    .HasForeignKey(w => w.ServiceTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(w => w.Offers)
                    .WithOne(o => o.Worker)
                    .HasForeignKey(o => o.WorkerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(w => w.Reviews)
                    .WithOne(r => r.Worker)
                    .HasForeignKey(r => r.WorkerId)
                    .OnDelete(DeleteBehavior.Restrict);

                
                entity.HasMany(w => w.AcceptedJobs)
                    .WithOne(j => j.AcceptedWorker)
                    .HasForeignKey(j => j.AcceptedWorkerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(w => w.Rating)
                    .HasDefaultValue(0.0)
                    .HasColumnType("decimal(3,2)");

                entity.Property(w => w.CompletedJobsCount)
                    .HasDefaultValue(0);

                entity.Property(w => w.IsVerified)
                    .HasDefaultValue(false);

              
                entity.Property(w => w.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

            });
            builder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");

                entity.HasKey(c => c.Id);

                entity.HasOne(w => w.User)
               .WithOne()
               .HasForeignKey<Customer>(w => w.UserId)
               .OnDelete(DeleteBehavior.Cascade);


                entity.HasMany(c => c.SavedWorkers)
                    .WithMany()
                    .UsingEntity<Dictionary<string, object>>(
                        "CustomerSavedWorkers",
                        j => j.HasOne<Worker>().WithMany().HasForeignKey("WorkerId").OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.Cascade)
                    );

                
                entity.HasMany(c => c.JobPosts)
                    .WithOne(j => j.Customer)
                    .HasForeignKey(j => j.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                
                entity.Property(c => c.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

               
            });
            builder.Entity<JobPost>(entity =>
            {
                entity.ToTable("JobPosts");

                entity.HasKey(j => j.Id);

               
                entity.HasOne(j => j.serviceType)
                    .WithMany(s => s.JobPosts)
                    .HasForeignKey(j => j.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);

      
                entity.HasOne(j => j.AcceptedWorker)
                    .WithMany(w => w.AcceptedJobs)
                    .HasForeignKey(j => j.AcceptedWorkerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

            
                entity.HasMany(j => j.Offers)
                    .WithOne(o => o.JobPost)
                    .HasForeignKey(o => o.JobPostId)
                    .OnDelete(DeleteBehavior.Cascade);

                
                entity.Property(j => j.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(j => j.Description)
                    .HasMaxLength(2000);

                entity.Property(j => j.Budget)
                    .HasColumnType("decimal(18,2)");

                entity.Property(j => j.Location)
                    .HasMaxLength(500);

                entity.Property(j => j.Status)
                    .HasDefaultValue(JobStatus.Pending);

                entity.Property(j => j.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(j => j.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                

                
                entity.HasIndex(j => j.Status);
                entity.HasIndex(j => j.ServiceId);
                entity.HasIndex(j => j.CreatedAt);
            });
            builder.Entity<Offer>(entity =>
            { 
                entity.ToTable("Offers");

                entity.HasKey(o => o.Id);

             
                entity.HasOne(o => o.JobPost)
                    .WithMany(j => j.Offers)
                    .HasForeignKey(o => o.JobPostId)
                    .OnDelete(DeleteBehavior.Cascade);

           
                entity.HasOne(o => o.Worker)
                    .WithMany(w => w.Offers)
                    .HasForeignKey(o => o.WorkerId)
                    .OnDelete(DeleteBehavior.Restrict);

               
                entity.Property(o => o.ProposedPrice)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(o => o.ProposedTimeline)
                    .HasMaxLength(100);

                entity.Property(o => o.Notes)
                    .HasMaxLength(1000);

                entity.Property(o => o.Status)
                    .HasDefaultValue(OfferStatus.Pending);

                entity.Property(o => o.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(o => o.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

               
                entity.HasIndex(o => new { o.JobPostId, o.WorkerId })
                    .IsUnique();

                
                entity.HasIndex(o => o.Status);
                entity.HasIndex(o => o.WorkerId);
           });
            builder.Entity<Review>(entity =>
            { 
                entity.ToTable("Reviews");

                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Customer)
                    .WithMany(r =>r.Reviews)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

       
                entity.HasOne(r => r.Worker)
                    .WithMany(w => w.Reviews)
                    .HasForeignKey(r => r.WorkerId)
                    .OnDelete(DeleteBehavior.Restrict);

         
                entity.Property(r => r.Rating)
                    .IsRequired();

                entity.Property(r => r.Comment)
                    .HasMaxLength(2000);

                entity.Property(r => r.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                
                entity.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5");

                
                entity.HasIndex(r => r.WorkerId);
                entity.HasIndex(r => r.Rating);
                entity.HasIndex(r => r.CreatedAt);
            });
            builder.Entity<ServiceType>(entity =>
            {
                entity.ToTable("ServiceTypes");

                entity.HasKey(s => s.Id);

                
                entity.HasMany(s => s.JobPosts)
                    .WithOne(j => j.serviceType)
                    .HasForeignKey(j => j.ServiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);


                entity.Property(s => s.Description)
                    .HasMaxLength(1000);

                entity.Property(s => s.IsActive)
                    .HasDefaultValue(true);

                entity.Property(s => s.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

              
                entity.HasIndex(s => s.Name)
                    .IsUnique();

                entity.HasIndex(s => s.IsActive);
            });



        }
    }

}
