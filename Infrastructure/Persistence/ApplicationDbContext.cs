using EyeOfHorusP.Domain.Configurations;
using EyeOfHorusP.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EyeOfHorusP.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // استدعاء الأساس أولًا

            // تطبيق جميع التهيئات الموجودة في نفس الـ Assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // ✅ إزالة `RoleConfiguration` لمنع التكرار
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        }
    }
}
