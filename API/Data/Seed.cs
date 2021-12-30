using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class Seed
    {
        // public static async Task SeedUsers(DataContext context) // Commented after implementing the Identity
        public static async Task SeedUsers(UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager)
        {
            // if(await context.Users.AnyAsync()) return; // Commented after implementing the Identity
            if(await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if(users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole {Name = "Member"},
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Moderator"}
            };

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach(var user in users)
            {
                //// Commented after implementing the Identity
                // using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                //// Commented after implementing the Identity
                // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                // user.PasswordSalt = hmac.Key;
                
                // await context.Users.AddAsync(user); // Commented after implementing the Identity
                 await userManager.CreateAsync(user, "Pa$$w0rd");
                 await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            // Below method is not Addto"Role", and Addto"Roles", that's why string array in second argument
            await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
            // Commented after implementing the Identity, UserManager taking care of saving the changes
            // await context.SaveChangesAsync(); 
            
        }
    }
}
