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
        
        /// <summary>
        /// User reserved id
        /// </summary>
        public Guid UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }
    }
}