using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(150, MinimumLength = 2)]
        public string MiddleName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }
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