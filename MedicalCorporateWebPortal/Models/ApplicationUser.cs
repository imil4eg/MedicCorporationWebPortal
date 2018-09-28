using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле логин обязательное")]
        [DisplayName("Логин")]
        [MinLength(6, ErrorMessage = "Минимальная длина 6 символов")]
        public override string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле пароль обязательное")]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Минимальная длина 6 символов")]
        public string Password { get; set; }

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
        public override string Email { get; set; }

        [DisplayName("Номер телефона")]
        [StringLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        [MinLength(11, ErrorMessage = "Длина номера телефона - 11 символов")]
        public override string PhoneNumber { get; set; }

        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Пациент,
        Врач,
        Администратор,
        Бухгалтер,

        Ресепшен
    }

    public enum Gender
    {
        Мужской,
        Женский
    }
}