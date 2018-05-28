using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalCorporateWebPortal.Models
{
    public class Patient
    {
        [Key]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public string Address { get; set; }

        [StringLength(11,MinimumLength = 11)]
        public string Phone { get; set; }

        [StringLength(4, MinimumLength = 4)]
        public string PassportSeries { get; set; }

        [StringLength(6, MinimumLength = 6)]
        public string PassportNumber { get; set; }

        [StringLength(11, MinimumLength = 11)]
        public string SNILS { get; set; }

        public string InsuranceNumber { get; set; }
        public string InsuranceCompany { get; set; }
    }
}