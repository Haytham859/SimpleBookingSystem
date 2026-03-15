using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWithOutArck.Configurations;
using ProjectWithOutArck.Date;
using ProjectWithOutArck.Dtos;
using ProjectWithOutArck.Models;

namespace ProjectWithOutArck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly BookingDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(BookingDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto register)
        {
            if(await _context.Users.AnyAsync(a=>a.Email==register.Email))
            {
                return BadRequest(new {Message="Email is already exist"});

            }


            var user = new User
            {
                Name = register.Name,
                Email = register.Email,
                Role = "User",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var result = new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Id = user.Id
            };
            return CreatedAtAction(nameof(Register), result);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
           var user= await _context.Users.FirstOrDefaultAsync(a => a.Email == login.Email);
          
            if(user is null)
            {
                return Unauthorized(new { Message = "Invalid Credintials " });
            }
            if (!BCrypt.Net.BCrypt.Verify(login.Password ,user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid Credintials " });

            }
            var token = GenerateToken.GenerateJwt(user, _config);


            var result = new
            {
                token,
                user = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                }
            };
            return Ok(result);
        }

    }
}
