using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class ApplicationIdentitySeed
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Badr Saeed",
                    Email = "badrsaeed85@gmail.com",
                    UserName = "BadrSaeed",
                    PhoneNumber = "01147137203",
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
