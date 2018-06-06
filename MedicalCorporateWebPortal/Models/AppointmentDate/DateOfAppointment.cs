using System;
using System.ComponentModel;
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
        [DisplayName("Дата")]
        public DateTime Date { get; set; }      

        [Required]
        [DisplayName("Период работы")]
        public string PeriodOfWorking { get; set; }
    }
}