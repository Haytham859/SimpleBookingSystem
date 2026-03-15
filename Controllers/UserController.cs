using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWithOutArck.Date;
using ProjectWithOutArck.Models;

namespace ProjectWithOutArck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly BookingDbContext _context;
        public UserController(BookingDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult>GetUsers([FromQuery]PaginationParameters pagination)
        {
            var query = _context.Set<User>().AsQueryable();

            var totalCount = await query.CountAsync();
            var users = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
            return Ok(new
            {
                totalCount,
                pagination.PageNumber,
                pagination.PageSize,
                data = users
            });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
