using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectWithOutArck.Date;
using ProjectWithOutArck.Models;

namespace ProjectWithOutArck.DataSeeding
{
    public class SeedData
    {
        private readonly BookingDbContext _db;
        public SeedData(BookingDbContext db)
        {
            _db = db;
        }
        public async Task Seeding()
        {
           if(!await _db.Users.AnyAsync())
            {
                var admin = new User
                {
                    Name = "admin",
                    Email = "adminclinc@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                    Role = "admin"
                };
                await _db.Users.AddAsync(admin);
                await _db.SaveChangesAsync();
            }
        }


    }
}
