using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PayCompute.Persistence
{
    public static class DataSendingInitializer
    {
        public static async Task UserAndRoleSeedAsync(UserManager<IdentityUser> userManager,
                                                      RoleManager<IdentityRole> roleManager)
        {
            // Array of group members ( Role )
            string[] roles = { "Admin", "Manager", "Staff" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            // Create Admin User
            if (userManager.FindByEmailAsync("admin@hotmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    /*
                     * Because of the current trend, we gonna use UserName as email, just like FANG
                     */
                    UserName = "admin@hotmail.com",
                    Email = "admin@hotmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Admin123").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            // Create Manager User
            if (userManager.FindByEmailAsync("manager@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    /*
                     * Because of the current trend, we gonna use UserName as email, just like FANG
                     */
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Manager123").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            // Create Staff User
            if (userManager.FindByEmailAsync("Jenny.doe@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    /*
                     * Because of the current trend, we gonna use UserName as email, just like FANG
                     */
                    UserName = "Jenny.doe@gmail.com",
                    Email = "Jenny.doe@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Staff123").Result;
                if (identityResult.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Staff").Wait();
                }
            }

            // Create No Role User
            if (userManager.FindByEmailAsync("john.doe@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    /*
                     * Because of the current trend, we gonna use UserName as email, just like FANG
                     */
                    UserName = "john.doe@gmail.com",
                    Email = "john.doe@gmail.com"
                };

                IdentityResult identityResult = userManager.CreateAsync(user, "Norole123").Result;
                //No role assigned to Mr John Doe
            }

        }
    }
}
