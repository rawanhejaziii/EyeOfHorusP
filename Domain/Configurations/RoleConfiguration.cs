using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace EyeOfHorusP.Domain.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(new List<IdentityRole>
            {
                new IdentityRole { Id = "b3b5e9a0-4f5b-4c6e-9c34-8d3a4d3c5a6a", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "1c7f3b64-2e8d-4e24-8c9c-d3cbe6789f6b", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "3d9c3a56-4b8e-49c2-bf8d-1a9fbc3d7e4d", Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Id = "f6a7c8e9-4d5b-42c6-b9c3-8d3a4d3c5b7a", Name = "Guest", NormalizedName = "GUEST" }
            });
        }
    }
}
