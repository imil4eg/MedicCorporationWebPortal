using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.AppData
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}