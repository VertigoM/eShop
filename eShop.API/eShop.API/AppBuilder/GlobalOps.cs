using eShop.API.Entities.Models;
using eShop.API.Services;
using Microsoft.AspNetCore.Identity;
using System.Drawing.Text;

namespace eShop.API.AppBuilder
{
    public static class GlobalOps
    {
        public static async Task CreateApplicationModerator(IServiceProvider serviceProvider)
        {
            try
            {
                var roleManager = serviceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                var userManager = serviceProvider
                    .GetRequiredService<UserManager<IdentityUser>>();

                IdentityResult identityResult;

                var roleExists = await roleManager
                    .RoleExistsAsync("Moderator");

                if (!roleExists)
                {
                    identityResult = await roleManager
                        .CreateAsync(new IdentityRole("Moderator"));
                }
                var user = await userManager
                    .FindByEmailAsync("moderator@domain.com");

                if (user == null)
                {
                    var defaultUser = new IdentityUser()
                    {
                        UserName = "moderator@domain.com",
                        Email = "moderator@domain.com"
                    };
                    var regUser = await userManager
                        .CreateAsync(defaultUser, "P@ssw0rd!");
                    await userManager
                        .AddToRoleAsync(defaultUser, "Moderator");
                }
            } 
            catch (Exception ex)
            {
                var str = ex.Message;
            }
        }

        public static async Task CreateDefaultUserRole(IServiceProvider serviceProvider)
        {
            try
            {
                var roleManager = serviceProvider
                   .GetRequiredService<RoleManager<IdentityRole>>();

                var userManager = serviceProvider
                    .GetRequiredService<UserManager<IdentityUser>>();

                IdentityResult identityResult;

                var roleExists = await roleManager
                    .RoleExistsAsync("User");

                if (!roleExists)
                {
                    identityResult = await roleManager
                        .CreateAsync(new IdentityRole("User"));
                }
                var user = await userManager
                    .FindByEmailAsync("test_user@domain.com");

                if (user == null)
                {
                    var defaultUser = new IdentityUser()
                    {
                        UserName = "test_user@domain.com",
                        Email = "test_user@domain.com"
                    };
                    var regUser = await userManager
                        .CreateAsync(defaultUser, "P@ssw0rd!");
                    await userManager
                        .AddToRoleAsync(defaultUser, "User");
                }
            }
            catch(Exception ex)
            {
                var str = ex.Message;
            }
        }
    }
}
