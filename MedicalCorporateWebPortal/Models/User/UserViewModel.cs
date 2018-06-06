using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class UserViewModel
    {
        [DisplayName("Идентификатор")]
        public Guid Id { get; set; }
        
        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия не может быть пустым")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя не может быть пустым")]
        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Эл. почта")]
        public string Email { get; set; }

        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Пол")]
        public Gender Gender { get; set; }

        [DisplayName("Роль")]
        public UserRole Role { get; set; }
    }
}
