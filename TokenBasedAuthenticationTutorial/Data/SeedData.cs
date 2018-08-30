using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuthenticationTutorial.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                AppUser user = new AppUser()
                {
                    Email="abc@mail.com",
                    SecurityStamp= Guid.NewGuid().ToString(),
                    UserName="Tindo"
                };
                userManager.CreateAsync(user, "Pa$$w0rd");

            }

        }
    }
}
