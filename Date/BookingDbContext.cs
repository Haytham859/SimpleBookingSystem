using Microsoft.EntityFrameworkCore;
using ProjectWithOutArck.Models;
using System.Reflection;
namespace ProjectWithOutArck.Date
{
    public class BookingDbContext:DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> context):base(context)
        {
            

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<User> Users{  get; set; }
        public DbSet<Doctor>Doctors{ get; set; }
        public DbSet<Appointment>Appointments{ get; set; }
    }
}
