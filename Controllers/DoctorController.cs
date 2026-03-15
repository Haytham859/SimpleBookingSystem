using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using ProjectWithOutArck.Date;
using ProjectWithOutArck.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ProjectWithOutArck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController:ControllerBase
    {
        private readonly BookingDbContext _context;
        public DoctorController(BookingDbContext context)
        {
            _context= context;
            
        }
        //Get with pagination and filtering
        [HttpGet]
        public async Task<IActionResult> GetDoctorsAsync([FromQuery]string? specialty, [FromQuery] PaginationParameters pagination)
        {
            var Query = _context.Doctors.AsQueryable();
            if(!string.IsNullOrEmpty(specialty))
            {
                Query = Query.Where(a => a.Specialty.Contains(specialty));


            }

            var totalCount = await Query.CountAsync();
            var doctors = await Query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return Ok(new
            {
                totalCount,
                pagination.PageNumber,
                pagination.PageSize,
                data = doctors
            });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetDocotroById(int?id)
        {
            if (id is null)
                return BadRequest();
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);


        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDocotroById), new { id = doctor.Id }, doctor);
        }
        [Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, Doctor updated)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            doctor.Name = updated.Name;
            doctor.Specialty = updated.Specialty;
            doctor.WorkStart = updated.WorkStart;
            doctor.WorkEnd = updated.WorkEnd;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
