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
                        Id = "6c7d12a2-bd8f-4f2e-bf4c-3b89d60f1fd4",
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN"
                    },
                    new IdentityRole
                    {
                        Id = "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6",
                        Name = "Driver",
                        NormalizedName = "DRIVER"
                    },
                    new IdentityRole
                    {
                        Id = "q7r8s9t0-u1v2-3w4x-5y6z-7a8b9c0d1e2f",
                        Name = "Salesperson",
                        NormalizedName = "SALESPERSON"
                    }
                );
    }
}
