using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.AppData
{
    public class AppointmentTime
    {
        [Key]
        public int ID { get; set; }

        public int DateOfAppointmentID { get; set; }
        [ForeignKey("DateOfAppointmentID")]
        public virtual DateOfAppointment DateOfAppointment { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public bool Reserved { get; set; }
    }
}