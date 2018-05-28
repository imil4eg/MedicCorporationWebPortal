using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class DoctorProvideService
    {
        [Key]
        public int DoctorProvideServiceID { get; set; }

        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public virtual Doctor Doctor { get; set; }

        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }
    }
}