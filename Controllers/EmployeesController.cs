using BarberApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarberApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm çalışanları listeleme
        //public async Task<IActionResult> Index()
        //{
        //    var employees = await _context.Employees.Include(e => e.EmployeeServices).ThenInclude(es => es.Service).ToListAsync();
        //    return View(employees);
        //}

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                                          .Include(e => e.Salon) // Salon bilgilerini dahil et
                                          .ToListAsync();
            return View(employees);
        }

        // Yeni çalışan ekleme
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    // Salonları dropdown için ViewBag ile gönderiyoruz
        //    ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
        //    return View();
        //}
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Çalışanı ekliyoruz
        //        _context.Employees.Add(employee);
        //        await _context.SaveChangesAsync();
        //        TempData["SuccessMessage"] = "Salon başarıyla kaydedildi.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else {
        //        TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
        //    }

        //    // Hata durumunda Salon listesini yeniden yükle
        //    ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
        //    return View(employee);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Employee employee)
        //{
        //    if (employee.SalonId == 0)
        //    {
        //        TempData["ErrorMessage"] = "Bir salon seçmelisiniz.";
        //        ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
        //        return View(employee);
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _context.Employees.Add(employee);
        //        await _context.SaveChangesAsync();
        //        TempData["SuccessMessage"] = "Çalışan başarıyla kaydedildi.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else if (!ModelState.IsValid)
        //    {
        //        var errors = ModelState.Values
        //                                .SelectMany(v => v.Errors)
        //                                .Select(e => e.ErrorMessage)
        //                                .ToList();
        //        TempData["ErrorMessage"] = "Hatalar: " + string.Join(", ", errors);
        //        ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
        //        return View(employee);
        //    }
        //    TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
        //    ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
        //    return View(employee);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            // Formdan gelen veriyi kontrol etmek için:
            var formData = Request.Form;
            var salonId = formData["SalonId"];
            var name = formData["Name"];
            var expertise = formData["Expertise"];
            

            // Geliştiriciye bilgi vermek için loglama veya hata mesajı eklemek
            Console.WriteLine($"SalonId: {salonId}, Name: {name}, Expertise: {expertise}");

            if (!ModelState.IsValid)
            {
                // Hata mesajlarını kullanıcıya göstermek için
                var errors = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();
                TempData["ErrorMessage"] = "Hatalar: " + string.Join(", ", errors);
                ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");
                return View(employee);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Çalışan başarıyla kaydedildi.";
            return RedirectToAction(nameof(Index));
        }
        // Çalışan detaylarını düzenleme
        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            // Salonlar ViewBag üzerinden gönderiliyor
            ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");

            return View(employee);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Çalışanı güncelle
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Çalışan başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            // Hata durumunda salonları tekrar View'a gönder
            ViewBag.Salons = new SelectList(_context.Salons.ToList(), "SalonId", "Name");

            return View(employee);
        }

        // Çalışan silme
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
