using System;

namespace MedicalCorporateWebPortal.Models
{
    public class PatientViewModel
    {
        public Guid PatientId { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }

        public int ServiceId { get; set; }
    }
}
