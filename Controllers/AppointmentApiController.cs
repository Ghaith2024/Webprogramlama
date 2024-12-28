using Microsoft.AspNetCore.Mvc;
using BarberApp.Models; // Modelinizi ekleyin
using System.Collections.Generic;
using System.Linq;

namespace BarberApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentApi
        [HttpGet]
        public IActionResult GetAppointments()
        {
            var appointments = _context.Appointments
                .Select(a => new
                {
                    a.AppointmentId,
                    a.AppointmentDate,
                    EmployeeName = a.Employee.Name,
                    a.IsConfirmed,
                    a.TotalPrice
                }).ToList();

            return Ok(appointments);
        }

        // GET: api/AppointmentApi/{id}
        [HttpGet("{id}")]
        public IActionResult GetAppointment(int id)
        {
            var appointment = _context.Appointments
                .Where(a => a.AppointmentId == id)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.AppointmentDate,
                    EmployeeName = a.Employee.Name,
                    a.IsConfirmed,
                    a.TotalPrice
                }).FirstOrDefault();

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }


        // POST: api/AppointmentApi
        [HttpPost]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
        }

        // PUT: api/AppointmentApi/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAppointment(int id, [FromBody] Appointment updatedAppointment)
        {
            if (id != updatedAppointment.AppointmentId)
            {
                return BadRequest("Appointment ID mismatch.");
            }

            var existingAppointment = _context.Appointments.Find(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            existingAppointment.AppointmentDate = updatedAppointment.AppointmentDate;
            existingAppointment.EmployeeId = updatedAppointment.EmployeeId;
            existingAppointment.IsConfirmed = updatedAppointment.IsConfirmed;
            existingAppointment.TotalPrice = updatedAppointment.TotalPrice;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/AppointmentApi/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
