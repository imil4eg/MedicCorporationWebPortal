using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class RecordConfirmViewModel
    {
        public int DoctorID { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorSpeciality { get; set; }

        public DateTime Date { get; set; }

        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public decimal ServiceCost { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле фамилия обязательное")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле имя обязательное")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Эл. почта")]
        [DataType(DataType.EmailAddress)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле эл. почта обязательно")]
        public string Email { get; set; }

        [DisplayName("Телефон")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(11, ErrorMessage = "Длина номера телефона 11 символов")]
        [MaxLength(11, ErrorMessage = "Длина номера телефона 11 символов")]
        public string Phone { get; set; }
    }
}
