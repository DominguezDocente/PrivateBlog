using PrivateBlog.Application.Contracts.Security;
using System;
using System.Collections.Generic;
using System.Text;
using static PrivateBlog.Application.Contracts.Security.PermissionCodesCatalog;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Persistence.Seeding
{
    public class PermissionsSeeder
    {
        private readonly DataContext _context;

        public PermissionsSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            foreach (PermissionSeed permission in PermissionCodesCatalog.All)
            {
                bool exists = await _context.Permissions.AnyAsync(p => p.Code == permission.Code);

                if (exists)
                {
                    continue;
                }

                await _context.AddAsync(new Permission(permission.Code, permission.Description, permission.Module));
                await _context.SaveChangesAsync();
            }
        }
    }
}
