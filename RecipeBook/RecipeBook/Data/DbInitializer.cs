using RecipeBook.Areas.Identity.Models;
using RecipeBook.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using RecipeBook.Models;

namespace RecipeBook.Data
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>(),
                                                        serviceProvider.GetRequiredService<IHttpContextAccessor>()))
            using (var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>())
            using (var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>())
            {

                var houseFish = await EnsureHousehold(context, "Fisher House");
                _ = await EnsureHousehold(context, "Maxi House");
                _ = await EnsureHousehold(context, "Mouse House");

                _ = await EnsureRole(roleManager, Role.Admin);
                _ = await EnsureRole(roleManager, Role.Standard);
                _ = await EnsureRole(roleManager, Role.ReadOnly);

                var userGraeme = await EnsureUser(context, userManager, "graeme.dg.corrin@gmail.com", "Graeme", "Corrin", "Test12#$", houseFish);
                _ = await EnsureUserRole(userManager, "graeme.dg.corrin@gmail.com", Role.Admin);

                await EnsureUnits(context);

            }
        }

        private static async Task<Household> EnsureHousehold(ApplicationContext context, string householdName)
        {
            var household = await context.Household.Where(x => x.Name == householdName).SingleOrDefaultAsync();

            if (household is null)
            {
                household = new Household() { Name = householdName };
                context.Add(household);
                await context.SaveChangesAsync();
            }

            return household;
        }

        private static async Task<AppRole> EnsureRole(RoleManager<AppRole> roleManager, string rolename)
        {
            var role = await roleManager.FindByNameAsync(rolename);

            if (role == null)
            {
                role = new AppRole(rolename);
                await roleManager.CreateAsync(role);
            }
            return role;
        }

        private static async Task<AppUser> EnsureUser(ApplicationContext context, UserManager<AppUser> userManager, string username, string firstName, string lastName, string password, Household household)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                };
                _ = await userManager.CreateAsync(user, password);

                user.Household = household;

                context.AppUser.Update(user);
                await context.SaveChangesAsync();

            }
            return user;
        }

        private static async Task<IdentityResult> EnsureUserRole(UserManager<AppUser> userManager, string username, string role)
        {
            var user = await userManager.FindByEmailAsync(username);

            if (user == null)
            {
                throw new Exception($"Cannot assign role.  User: {username} does not exist");
            }

            return await userManager.AddToRoleAsync(user, role);
        }
    
        private static async Task EnsureUnits(ApplicationContext context)
        {
            if (!context.Unit.Any())
            {
                context.Unit.AddRange(new Unit[]
                {
                    new Unit { Name = "tsp" },
                    new Unit { Name = "tbsp" },
                    new Unit { Name = "fl oz" },
                    new Unit { Name = "c" },
                    new Unit { Name = "ml" },
                    new Unit { Name = "l" },
                    new Unit { Name = "lb" },
                    new Unit { Name = "oz" },
                    new Unit { Name = "mg" },
                    new Unit { Name = "g" },
                    new Unit { Name = "kg" },
                });
            }

            await context.SaveChangesAsync();

        }
    
    }
}
