using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class ProfileViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле фамилия обязательное")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле имя обязательное")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Выберете пол")]
        [DisplayName("Пол")]
        public Gender Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле эл. почта обязательное")]
        [DisplayName("Эл. почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Номер телефона")]
        [StringLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [MinLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Номер телефона может состоять только из цифр")]
        public string PhoneNumber { get; set; }

        [DisplayName("Адрес")]
        public string Address { get; set; }

        [DisplayName("Серия паспорта")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Длина серии паспорта 4 символа")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Серия паспорта может состоять только из цифр")]
        public string PassportSeries { get; set; }

        [DisplayName("Серия паспорта")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Длина номера паспорта 6 символов")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Номер паспорта может состоять только из цифр")]
        public string PassportNumber { get; set; }

        [DisplayName("СНИЛС")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Длина СНИЛС 11 сиволов")]
        public string SNILS { get; set; }

        [DisplayName("Номер страхового полиса")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Номер страхового полиса может состоять только из цифр")]
        public string InsuranceNumber { get; set; }

        [DisplayName("Название страховой компании")]
        public string InsuranceCompany { get; set; }
    }
}
