using BarberApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BarberApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Name { get; set; } // Çalışan adı
        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        public string Expertise { get; set; } // Çalışanın uzmanlık alanı
        public int? SalonId { get; set; } // Çalışanın bağlı olduğu salon
        public Salon? Salon { get; set; } // Çalışanın bağlı olduğu salon

        //public ICollection<EmployeeService> EmployeeServices { get; set; } // Hangi hizmetleri verebilir
        public ICollection<Appointment> Appointments { get; set; }

        public Employee()
        {
            //EmployeeServices = new List<EmployeeService>();
            Appointments = new List<Appointment>();
        }/// Çalışanın aldığı randevular
    }
}
