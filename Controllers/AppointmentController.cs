using BarberApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberApp.Controllers
{
    [Authorize] // Randevulara sadece giriş yapmış kullanıcılar erişebilir
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Kullanıcı: Kendi randevularını listeleme
        public async Task<IActionResult> MyAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            var appointments = await _context.Appointments
                .Include(a => a.Employee)
                //.Include(a => a.AppointmentServices)
                //.ThenInclude(ee => ee.Service)
                /*.Where(a => a.UserId == user.Id)*/ // UserId'nin doğru şekilde kullanıldığından emin olun
                .ToListAsync();

            return View(appointments);
        }

        // Kullanıcı: Yeni randevu talebi
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            //ViewBag.Services = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var user = await _userManager.GetUserAsync(User);
            /*appointment.UserId = user.Id;*/ // Kullanıcı ID'sini doğru şekilde ayarlayın
            appointment.IsConfirmed = false;

            // Randevu için seçilen hizmetleri ekleyelim
            //foreach (var serviceId in selectedServices)
            //{
            //    var appointmentService = new AppointmentService
            //    {
            //        Appointment = appointment,
            //        ServiceId = serviceId
            //    };
            //    _context.AppointmentServices.Add(appointmentService);
            //}

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyAppointments));
        }

        // Admin: Tüm randevuları listeleme
        [Authorize(Roles = "Admin")] // Sadece adminler bu sayfayı görebilir
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User) // Kullanıcıyı dahil ediyoruz
                .Include(a => a.Employee)
                //.Include(a => a.AppointmentServices)
                //.ThenInclude(ee => ee.Service)
                .ToListAsync();

            return View(appointments);
        }

        // Admin: Randevuyu onaylama
        [Authorize(Roles = "Admin")] // Sadece adminler bu işlemi yapabilir
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.IsConfirmed = true;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
