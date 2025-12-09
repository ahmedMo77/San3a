using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using San3a.Core.Entities;
using Microsoft.EntityFrameworkCore;
using San3a.Core.Base;

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
        public DbSet<Offer> Offers { get; set; }
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<CraftsmanPortfolio> CraftsmanPortfolios { get; set; }
        public DbSet<CraftsmanPortfolioImage> PortfolioImages { get; set; }
        public DbSet<PortfolioRequest> PortfolioRequests { get; set; }
        public DbSet<JobAttachment> JobAttachments { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseAuditableEntity || e.Entity is AppUser);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is BaseAuditableEntity auditableEntity)
                    {
                        auditableEntity.CreatedAt = DateTime.UtcNow;
                        auditableEntity.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (entry.Entity is AppUser appUser)
                    {
                        appUser.CreatedAt = DateTime.UtcNow;
                        appUser.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is BaseAuditableEntity auditableEntity)
                    {
                        auditableEntity.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (entry.Entity is AppUser appUser)
                    {
                        appUser.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
        }

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

            builder.Entity<Customer>()
                .HasIndex(c => c.NationalId)
                .IsUnique()
                .HasFilter("[NationalId] IS NOT NULL");

            builder.Entity<Craftsman>()
                .HasOne(c => c.AppUser)
                .WithOne()
                .HasForeignKey<Craftsman>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Craftsman>()
                .HasOne(c => c.Service)
                .WithMany(s => s.Craftsmen)
                .HasForeignKey(c => c.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.Customer)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Job>()
                .HasOne(j => j.ServiceType)
                .WithMany(s => s.Jobs)
                .HasForeignKey(j => j.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.AcceptedWorker)
                .WithMany(c => c.AcceptedJobs)
                .HasForeignKey(j => j.AcceptedCraftsmanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.DirectCraftsman)
                .WithMany(c => c.DirectJobs)
                .HasForeignKey(j => j.DirectCraftsmanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .Property(j => j.Status)
                .HasConversion<string>();

            builder.Entity<Job>()
                .Property(j => j.PostingType)
                .HasConversion<string>();

            builder.Entity<Offer>()
                .HasOne(o => o.Job)
                .WithMany(j => j.Offers)
                .HasForeignKey(o => o.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Offer>()
                .HasOne(o => o.Worker)
                .WithMany(c => c.Offers)
                .HasForeignKey(o => o.CraftsmanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Offer>()
                .Property(o => o.Status)
                .HasConversion<string>();

            builder.Entity<JobRequest>()
                .HasOne(jr => jr.Job)
                .WithMany(j => j.DirectRequests)
                .HasForeignKey(jr => jr.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<JobRequest>()
                .HasOne(jr => jr.Craftsman)
                .WithMany(c => c.JobRequests)
                .HasForeignKey(jr => jr.CraftsmanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobRequest>()
                .Property(jr => jr.Status)
                .HasConversion<string>();

            builder.Entity<JobAttachment>()
                .HasOne(ja => ja.Job)
                .WithMany(j => j.Attachments)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CraftsmanPortfolio>()
                .HasOne(cp => cp.Craftsman)
                .WithMany(c => c.Portfolios)
                .HasForeignKey(cp => cp.CraftsmanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CraftsmanPortfolioImage>()
                .HasOne(cpi => cpi.Portfolio)
                .WithMany(cp => cp.Images)
                .HasForeignKey(cpi => cpi.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PortfolioRequest>()
                .HasOne(pr => pr.Portfolio)
                .WithMany(cp => cp.Requests)
                .HasForeignKey(pr => pr.PortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PortfolioRequest>()
                .HasOne(pr => pr.Customer)
                .WithMany(c => c.PortfolioRequests)
                .HasForeignKey(pr => pr.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PortfolioRequest>()
                .Property(pr => pr.Status)
                .HasConversion<string>();
        }
    }
}