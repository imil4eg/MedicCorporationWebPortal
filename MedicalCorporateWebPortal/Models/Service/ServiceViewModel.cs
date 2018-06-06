using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class ServiceViewModel
    {
        public int ServiceID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле название обязательное")]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле цена обязательное")]
        [DisplayName("Цена")]
        public decimal Price { get; set; }
    }
}
