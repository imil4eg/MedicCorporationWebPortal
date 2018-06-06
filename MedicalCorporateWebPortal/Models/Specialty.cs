using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class Specialty
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
