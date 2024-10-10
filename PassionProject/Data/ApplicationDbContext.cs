using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PassionProject.Models;
using System.Numerics;

namespace PassionProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //Car.cs will map to a Cars Table
        public DbSet<Car> Cars { get; set; }

        //Staff.cs will map to a Staffs Table
        public DbSet<Staff> Staffs { get; set; }

        //Owner.cs will map to a Owners Table
        public DbSet<Owner> Owners { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
