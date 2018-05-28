using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class ServicesHistory
    {
        [Key]
        public int ID { get; set; }

        public int PatientID { get; set; }
        [ForeignKey("PatientID")]
        public virtual Patient Patient { get; set; }

        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }

        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public virtual Doctor Doctor { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Conclustion { get; set; }
    }
}