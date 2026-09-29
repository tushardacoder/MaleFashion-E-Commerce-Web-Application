using Demo.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Seeds
{
    public class UserSeeds
    {
        public static ApplicationUser[] GetUsers()
        {
            return new ApplicationUser[]
            {
            new ApplicationUser
            {
                Id = Guid.Parse(
                    "00000000-0000-0000-0000-000000000010"),

                FirstName = "Admin",

                LastName = "User",

                DateOfBirth = new DateTime(
                    2001,
                    1,
                    1),

                UserName = "admin@malefashion.com",

                NormalizedUserName =
                    "ADMIN@MALEFASHION.COM",

                Email = "admin@malefashion.com",

                NormalizedEmail =
                    "ADMIN@MALEFASHION.COM",

                EmailConfirmed = true,

                PasswordHash =
                    "AQAAAAIAAYagAAAAEPVfuBMniXS4qIDvUEtH6ibwBNS90i1YycUUlNFfOGW8XMKrNijLFlT+I1ZP9wAMHg==",

                SecurityStamp =
                    "A8F5B6C7-D8E9-4F01-AB23-C45678901234",

                ConcurrencyStamp =
                    "B9A6C7D8-E9F0-4012-BC34-D56789012345",

                PhoneNumber = null,

                PhoneNumberConfirmed = false,

                TwoFactorEnabled = false,

                LockoutEnd = null,

                LockoutEnabled = true,

                AccessFailedCount = 0
            }
            };
        }
    }
}
