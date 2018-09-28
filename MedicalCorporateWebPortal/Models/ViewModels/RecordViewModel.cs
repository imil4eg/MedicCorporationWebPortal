using System;

namespace MedicalCorporateWebPortal.Models
{
    public class RecordViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public Doctor Doctor { get; set; }

        public DateTime Date { get; set; }

        public Service Service { get; set; }

        public ReservedTime ReservedTime { get; set; }
    }
}
