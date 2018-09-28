using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле эл. почта обязательное")]
        [DisplayName("Эл. почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле логин обязательное")]
        [DisplayName("Логин")]
        [MinLength(6, ErrorMessage = "Минимальная длина 6 символов")]
        public string Login { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле пароль обязательное")]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Минимальная длина 6 символов")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [DisplayName("Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле фамилия обязательное")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле имя обязательное")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Номер телефона")]
        [StringLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [MinLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Номер телефона может состоять только из цифр")]
        public string Phone { get; set; }

        [DisplayName("Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Выберете пол")]
        [DisplayName("Пол")]
        public Gender Gender { get; set; }
    }
}
