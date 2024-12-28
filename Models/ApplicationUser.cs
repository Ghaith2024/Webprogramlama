using Microsoft.AspNetCore.Identity;

namespace BarberApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Buraya kullanıcıya özel başka özellikler ekleyebilirsiniz
        public int UserId { get; set; }
        public string FullName { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}