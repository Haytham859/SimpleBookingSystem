using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectWithOutArck.Date;
using ProjectWithOutArck.Models;
using System.Security.Claims;

namespace ProjectWithOutArck.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public AppointmentsController(BookingDbContext context)
        {
            _context= context;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery]int? doctorId,
            [FromQuery]int? userId,
            [FromQuery]string?status,
            [FromQuery]PaginationParameters pagination
            )
        {
            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var query = _context.Appointments.Include(a => a.User).Include(a => a.Doctor).AsQueryable();

            if(role!="Admin")
            {
                query=query.Where(a=>a.UserId==UserId);

            }
            if (doctorId.HasValue)
                query = query.Where(a => a.DoctorId == doctorId);
            if (status is not null)
                query = query.Where(a => a.Status == status);

            var totalCount = await query.CountAsync();


            var appResult = await query.OrderByDescending(a => a.AppointmentDate)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.Status,
                    DoctorName=a.Doctor.Name,
                    UserName=a.User.Name
                }).ToListAsync();

            return Ok(new
            {
                totalCount
                ,
                pagination.PageNumber,
                pagination.PageSize,
                data = appResult
            });


        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();
            return Ok(appointment);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAppointment(Appointment appointment)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var exists = await _context.Appointments.AnyAsync(a => a.DoctorId == appointment.DoctorId && a.AppointmentDate == appointment.AppointmentDate);

            if (exists)
                return BadRequest(new { Message = "This Time Is Already Booked" });

            var appResult = new Appointment
            {
                DoctorId = appointment.DoctorId,
                UserId = userId,
                AppointmentDate = appointment.AppointmentDate,
                Status = "Pending"
            };

            await _context.Appointments.AddAsync(appResult);
            await _context.SaveChangesAsync();
            return Ok(appResult);


        }
        [Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, Appointment updated)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = updated.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {

            var role = User.FindFirst(ClaimTypes.Role)!.Value;
            var UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var appResult=await _context.Appointments.FindAsync(id);
            if (appResult == null)
                return NotFound();
            if (role != "Admin" && appResult.Id != UserId)
                return Forbid();
            _context.Remove(appResult);
            await _context.SaveChangesAsync();

            return NoContent();

        }


    }
}
