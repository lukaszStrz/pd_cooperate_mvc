using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cooperate_mvc.Models
{
    public class TaskModel
    {
        /// <summary>
        /// Czy task dla mnie (true), czy zlecony przeze mnie (false)
        /// </summary>
        public bool ToMe { get; set; }

        #region Task

        public long Id { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(50, ErrorMessage = "Nie może być dłuższe niż 50 znaków")]
        [Display(Name = "Tytuł zadania")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [MaxLength(500, ErrorMessage = "Nie może być dłuższe niż 500 znaków")]
        [Display(Name = "Opis zadania")]
        public string Description { get; set; }

        [Display(Name = "Data utworzenia")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [Display(Name = "Termin wykonania")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Display(Name = "Ostatnio zmienione")]
        public DateTime StatusLastChange { get; set; }

        #endregion

        #region User

        public int UserFromId { get; set; }

        [Display(Name = "Zlecone przez")]
        public string UserFrom_login { get; set; }

        public int UserTo_id { get; set; }

        [Display(Name = "Zlecone dla")]
        public string UserTo_login { get; set; }

        public int UserStatusChangedBy_id { get; set; }

        [Display(Name = "Zmienione przez")]
        public string User_login_statusChangedBy { get; set; }

        #endregion

        #region Status

        public byte TaskStatus_id { get; set; }

        [Display(Name = "Status")]
        public string TaskStatus_name { get; set; }

        #endregion

        #region Group

        public long Group_id { get; set; }

        [Display(Name = "W grupie")]
        public string Group_name { get; set; }

        #endregion
    }
}