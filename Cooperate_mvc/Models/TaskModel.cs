using System;
using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class TaskModel
    {
        /// <summary>
        /// Czy task dla mnie (true), czy zlecony przeze mnie (false)
        /// </summary>
        public bool ToMe { get; set; }

        public long Id { get; set; }
        public int User_from { get; set; }
        public int User_to { get; set; }
        public byte TaskStatus_id { get; set; }
        public long Group_id { get; set; }
        public int User_statusChangedBy { get; set; }

        [Display(Name = "W grupie")]
        public string Group { get; set; }

        [Display(Name = "Zlecone przez")]
        public string User_login_from { get; set; }

        [Display(Name = "Zlecone dla")]
        public string User_login_to { get; set; }

        [Display(Name = "Zmienione przez")]
        public string User_login_statusChangedBy { get; set; }

        [Display(Name = "Status")]
        public string TaskStatus { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [Display(Name = "Tytuł zadania")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(500, ErrorMessage = "Nie może być dłuższe niż 500 znaków")]
        [Display(Name = "Opis zadania")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [Display(Name = "Data utworzenia")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [Display(Name = "Termin wykonania")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Ostatnio zmienione")]
        public DateTime Task_statusLastChange { get; set; }
    }
}