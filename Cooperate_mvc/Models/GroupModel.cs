using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class GroupModel
    {
        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [Display(Name="Nazwa grupy")]
        public string Group_name { get; set; }

        [MaxLength(500, ErrorMessage = "Nie może być dłuższe niż 500 znaków")]
        [Display(Name = "Opis grupy")]
        public string Group_description { get; set; }
    }
}