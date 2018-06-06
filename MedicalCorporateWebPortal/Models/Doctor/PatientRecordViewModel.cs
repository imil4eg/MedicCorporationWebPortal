using System;

namespace MedicalCorporateWebPortal.Models
{
    public class PatientRecordViewModel
    {
        public Guid PatientID { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }

        public DateTime Time { get; set; }
        public int ReservedTimeId { get; set; }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}
