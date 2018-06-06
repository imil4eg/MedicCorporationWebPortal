using System;

namespace MedicalCorporateWebPortal.Models
{
    public class RecordViewModel
    {
        public User User { get; set; }

        public Doctor Doctor { get; set; }

        public DateTime Date { get; set; }

        public Service Service { get; set; }

        public ReservedTime ReservedTime { get; set; }
    }
}
