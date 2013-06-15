using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cooperate_mvc.Models
{
    public class TaskModel
    {
        //compact_dbEntities db = new compact_dbEntities();

        //public List<GroupModel> GetGroups(string UserLogin)
        //{
        //    return (from g in db.Groups
        //            join p in db.Participations on g.Group_id equals p.Group_id
        //            join u in db.Users on p.User_id equals u.User_id
        //            where u.User_login.Equals(UserLogin)
        //            select new GroupModel()
        //            {
        //                IsAdmin = p.Participation_isAdmin,
        //                CreationDate = g.Group_creationDate,
        //                Description = g.Group_description,
        //                Id = g.Group_id,
        //                Name = g.Group_name
        //            }).ToList();
        //}

        //public List<UserModel> GetUsers(long forGroup, string UserMeLogin)
        //{
        //    return (from u in db.Users
        //            join p in db.Participations on u.User_id equals p.User_id
        //            join g in db.Groups on p.Group_id equals g.Group_id
        //            where g.Group_id.Equals(forGroup) && !u.User_login.Equals(UserMeLogin)
        //            select new UserModel()
        //            {
        //                Birth = u.User_birth,
        //                Email = u.User_email,
        //                FirstName = u.User_firstName,
        //                LastName = u.User_lastName,
        //                Login = u.User_login,
        //                Id = u.User_id
        //            }).ToList();
        //}

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
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Pole obowiązkowe")]
        [Display(Name = "Termin wykonania")]
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