namespace BarberApp.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; } // Randevu talep eden kullanıcı
        /*public User User { get; set; } */// Kullanıcı
        public ApplicationUser User { get; set; }
        public int EmployeeId { get; set; } // Randevuyu kabul eden çalışan
        public Employee Employee { get; set; }
        public DateTime AppointmentDate { get; set; } // Randevu tarihi
        public bool IsConfirmed { get; set; } // Onay durumu
        public decimal TotalPrice { get; set; } // Toplam ücret

        /*public ICollection<AppointmentService> AppointmentServices { get; set; }*/ // Hangi hizmetler seçildi
    }
}
