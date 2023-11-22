using Microsoft.EntityFrameworkCore;
using SPSApps.Models.Parking;
using SPSApps.Models.Register;

namespace SPSApps.Models
{
    public class DatabaseEntity : DbContext
    {
        public DatabaseEntity(DbContextOptions<DatabaseEntity> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<RequestParking> RequestParkings { get; set; } 
    }
}
