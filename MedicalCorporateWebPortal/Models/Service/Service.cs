using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}