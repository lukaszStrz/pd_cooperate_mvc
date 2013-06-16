using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(100, ErrorMessage = "Nie może być dłuższe niż 100 znaków")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(100, ErrorMessage = "Nie może być dłuższe niż 100 znaków")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [Display(Name = "Data urodzenia")]
        [DataType(DataType.Date)]
        public System.DateTime Birth { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [EmailAddress(ErrorMessage = "Wpisz prawidłowy adres email")]
        [MaxLength(254, ErrorMessage = "Nie może być dłuższe niż 254 znaków")]
        [System.Web.Mvc.Remote("MailExist", "Account", HttpMethod = "POST", ErrorMessage = "Istnieje już użytkownik z podanym adresem")]
        [Display(Name = "Adres e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [MinLength(4, ErrorMessage="Hasło musi mieć minimum 4 znaki")]
        [Display(Name = "Hasło")]
        public string Pass { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [System.Web.Mvc.Remote("LoginExist", "Account", HttpMethod = "POST", ErrorMessage = "Login zajęty")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [Compare("Pass", ErrorMessage = "Hasła nie zgadzają się")]
        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; }
    }
}