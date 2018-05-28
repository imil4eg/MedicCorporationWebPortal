using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class DateOfAppointment
    {
        [Key]
        public int DateOfAppointmentID { get; set; }

        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public virtual Doctor Doctor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string PeriodOfWorking { get; set; }
    }
}