using System.ComponentModel.DataAnnotations;

namespace BarberApp.Models
{
    public class Salon
    {
        public int SalonId { get; set; }

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        public string Name { get; set; } // Salon adı

        [Required(ErrorMessage = "Adres zorunludur.")]
        public string Address { get; set; } // Adres

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Phone { get; set; } // Telefon

        [Required(ErrorMessage = "Salon türü zorunludur.")]
        public string SalonType { get; set; } // Bay/Bayan salonu

        // Bu koleksiyonlar için [Required] kaldırılmalı
        public ICollection<Employee> Employees { get; set; } // Salondaki çalışanlar
        //public ICollection<Service> Services { get; set; }
        public Salon()
        {
            Employees = new List<Employee>();
            //Services = new List<Service>();
        }// Sunulan hizmetler
    }
}