using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class AppDBContext:IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
        }

        public DbSet<Directorate> Directorates { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<fridgeTempreture> fridgeTempretures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Clinic>()
                .HasOne<Directorate>()
                .WithMany()
                .HasForeignKey(p => p.DirectorateId);
            modelBuilder.Entity<ApplicationUser>()
               .HasOne<Directorate>()
               .WithMany()
               .HasForeignKey(p => p.DirectorateId);
            modelBuilder.Entity<ApplicationUser>()
             .HasOne<Clinic>()
             .WithMany()
             .HasForeignKey(p => p.clinicId);



            modelBuilder.Entity<fridgeTempreture>()
                   .HasOne<Clinic>()
                   .WithMany()
                   .HasForeignKey(p => p.clinicId);
        
        }
    }
}
