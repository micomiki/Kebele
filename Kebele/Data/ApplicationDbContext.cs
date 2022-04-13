using Kebele.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kebele.Data
{
    public class ApplicationDbContext : IdentityUserContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

         
    }
}
