using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class ReservedTime
    {
        [Key]
        public int ID { get; set; }

        public int DateOfAppointmentID { get; set; }
        [ForeignKey("DateOfAppointmentID")]
        public virtual DateOfAppointment DateOfAppointment { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int? UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}