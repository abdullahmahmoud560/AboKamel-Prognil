using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure.Configurations;

public class UserRolesConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
                    new IdentityRole
                    {
                        Id = "d1f0f030-8a2d-4d42-9c23-9f1ecb0f61c1", // static GUID
                        Name = "Customer",
                        NormalizedName = "CUSTOMER"
                    },
                    new IdentityRole
                    {
                        Id = "3d48c77e-38fd-4b0e-8c2d-3b10d63a35e0",
                        Name = "PharmacyAdmin",
                        NormalizedName = "PHARMACYADMIN"
                    },
                    new IdentityRole
                    {
                        Id = "6c7d12a2-bd8f-4f2e-bf4c-3b89d60f1fd4",
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN"
                    },
                    new IdentityRole
                    {
                        Id = "f3b82a73-3e4d-4b8c-a6b8-1f5a9b6d2b15",
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    }
                );
    }
}
