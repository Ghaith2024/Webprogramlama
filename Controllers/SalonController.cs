using BarberApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BarberApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var salons = _context.Salons.ToList();
            return View(salons);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Salon salon)
        {
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    TempData["ErrorMessage"] = "Hatalar: " + string.Join(", ", errors);
            //    return View(salon);
            //}
            if (ModelState.IsValid)
            {
                _context.Salons.Add(salon);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Salon başarıyla kaydedildi.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Bir hata oluştu. Lütfen tekrar deneyin.";
            return View(salon);
        }

        public IActionResult Edit(int id)
        {
            var salon = _context.Salons.Find(id);
            if (salon == null)
            {
                return NotFound();
            }
            TempData["SuccessMessage"] = "Salon başarıyla kaydedildi.";
            return View(salon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Salons.Update(salon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(salon);
        }

        public IActionResult Delete(int id)
        {
            var salon = _context.Salons.Find(id);
            if (salon != null)
            {
                _context.Salons.Remove(salon);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
