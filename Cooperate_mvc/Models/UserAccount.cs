using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class UserAccount
    {
        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(100, ErrorMessage = "Nie może być dłuższe niż 100 znaków")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(100, ErrorMessage = "Nie może być dłuższe niż 100 znaków")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        public System.DateTime Birth { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [EmailAddress(ErrorMessage = "Wpisz prawidłowy adres email")]
        [MaxLength(254, ErrorMessage = "Nie może być dłuższe niż 254 znaków")]
        [System.Web.Mvc.Remote("MailExist", "Account", HttpMethod = "POST", ErrorMessage = "Istnieje już użytkownik z podanym adresem")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [MinLength(4, ErrorMessage="Hasło musi mieć minimum 4 znaki")]
        public string Pass { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [System.Web.Mvc.Remote("LoginExist", "Account", HttpMethod = "POST", ErrorMessage = "Login zajęty")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [Compare("Pass", ErrorMessage = "Hasła nie zgadzają się")]
        public string ComparePassword { get; set; }
    }
}