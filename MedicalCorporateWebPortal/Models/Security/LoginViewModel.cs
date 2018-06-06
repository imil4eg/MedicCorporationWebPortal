using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MedicalCorporateWebPortal.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле логин обязательно")]
        [DisplayName("Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле пароль обязательно")]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}
