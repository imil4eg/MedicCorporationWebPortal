using System.Collections.Generic;
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле описания не может быть пустым")]
        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        public string Descripition { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле цена обязательное")]
        [DisplayName("Цена")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public IList<DoctorViewModel> Doctors { get; set; }
    }
}
