using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace User.Domain.Seeders;

public class UserSeeder(User.Domain.Repositories.DbContext context) : IUserSeeder
{
    public async Task Seed()
    {
        if (!context.Roles.Any())
        {
            var roles = new List<User.Domain.Models.Entities.Role>
            {
                new User.Domain.Models.Entities.Role { Id = 1, Name = "Client" },
                new User.Domain.Models.Entities.Role { Id = 2, Name = "Employee" },
                new User.Domain.Models.Entities.Role { Id = 3, Name = "Administrator" },
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var clientRole = context.Roles.First(r => r.Name == "Client");
            var employeeRole = context.Roles.First(r => r.Name == "Employee");
            var adminRole = context.Roles.First(r => r.Name == "Administrator");

            var users = new List<User.Domain.Models.Entities.User>
            {
                new User.Domain.Models.Entities.User { Id = 1, Username = "TW", Email = "TW@email.org", PasswordHash = "TWPasswordHash",  IsActive = true, Roles = new List<User.Domain.Models.Entities.Role>{ adminRole, employeeRole, clientRole} },
                new User.Domain.Models.Entities.User { Id = 2, Username = "AW", Email = "AW@email.org", PasswordHash = "AWPasswordHash",  IsActive = true, Roles = new List<User.Domain.Models.Entities.Role>{ employeeRole, clientRole} },
                new User.Domain.Models.Entities.User { Id = 3, Username = "LW", Email = "LW@email.org", PasswordHash = "LWPasswordHash",  IsActive = true, Roles = new List<User.Domain.Models.Entities.Role>{ clientRole} },

            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }

}
