using Cooperate_mvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Cooperate_mvc.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Tasks/

        public ActionResult Index()
        {
            var tasks = (from t in db.Tasks
                         join uFrom in db.Users on t.User_from equals uFrom.User_id
                         join uTo in db.Users on t.User_to equals uTo.User_id
                         join uChanged in db.Users on t.User_statusChangedBy equals uChanged.User_id
                         join ts in db.TaskStatus on t.TaskStatus_id equals ts.TaskStatus_id
                         join g in db.Groups on t.Group_id equals g.Group_id
                         where uFrom.User_login.Equals(User.Identity.Name) || uTo.User_login.Equals(User.Identity.Name)
                         select new TaskModel()
                         {
                             ToMe = uTo.User_login.Equals(User.Identity.Name),
                             CreationDate = t.Task_creationDate,
                             Deadline = t.Task_deadline,
                             Description = t.Task_description,
                             Group_id = t.Group_id,
                             Id = t.Task_id,
                             StatusLastChange = t.Task_statusLastChange,
                             TaskStatus_id = t.TaskStatus_id,
                             TaskStatus_name = ts.TaskStatus_name,
                             Title = t.Task_title,
                             UserFromId = t.User_from,
                             UserFrom_login = uFrom.User_login,
                             UserTo_id = t.User_to,
                             UserTo_login = uTo.User_login,
                             UserStatusChangedBy_id = t.User_statusChangedBy,
                             User_login_statusChangedBy = uChanged.User_login,
                             Group_name = g.Group_name
                         });

            return View(tasks.ToList());
        }

        //
        // GET: /Tasks/Details/5

        public ActionResult Details(long id = 0)
        {
            Task task = db.Tasks.SingleOrDefault(t => t.Task_id.Equals(id));
            if (task == null)
            {
                return HttpNotFound();
            }

            TaskModel tm = new TaskModel()
            {
                ToMe = task.User1.User_login.Equals(User.Identity.Name),
                CreationDate = task.Task_creationDate,
                Deadline = task.Task_deadline,
                Description = task.Task_description,
                Group_id = task.Group_id,
                Id = task.Task_id,
                StatusLastChange = task.Task_statusLastChange,
                TaskStatus_id = task.TaskStatus_id,
                TaskStatus_name = task.TaskStatu.TaskStatus_name,
                Title = task.Task_title,
                UserFromId = task.User_from,
                UserFrom_login = task.User.User_login,
                UserTo_id = task.User_to,
                UserTo_login = task.User1.User_login,
                UserStatusChangedBy_id = task.User_statusChangedBy,
                User_login_statusChangedBy = task.User2.User_login,
                Group_name = task.Group.Group_name
            };

            return View(tm);
        }

        private List<GroupModel> GetGroups(string UserLogin)
        {
            return (from g in db.Groups
                    join p in db.Participations on g.Group_id equals p.Group_id
                    join u in db.Users on p.User_id equals u.User_id
                    where u.User_login.Equals(UserLogin)
                    select new GroupModel()
                    {
                        IsAdmin = p.Participation_isAdmin,
                        CreationDate = g.Group_creationDate,
                        Description = g.Group_description,
                        Id = g.Group_id,
                        Name = g.Group_name
                    }).ToList();
        }

        private List<UserModel> GetUsers(long forGroup, string UserMeLogin)
        {
            return (from u in db.Users
                    join p in db.Participations on u.User_id equals p.User_id
                    join g in db.Groups on p.Group_id equals g.Group_id
                    where g.Group_id.Equals(forGroup) && !u.User_login.Equals(UserMeLogin)
                    select new UserModel()
                    {
                        Birth = u.User_birth,
                        Email = u.User_email,
                        FirstName = u.User_firstName,
                        LastName = u.User_lastName,
                        Login = u.User_login,
                        Id = u.User_id
                    }).ToList();
        }

        [HttpPost]
        public JsonResult GetUsersByGroupId(long id)
        {
            var result = GetUsers(id, User.Identity.Name);
            //build JSON.  
            var modelDataStore = from m in result
                                 select new[] { m.Id.ToString(),
                                                m.Login };

            return Json(modelDataStore, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Tasks/Create

        public ActionResult Create()
        {
            ViewBag.GroupsList = GetGroups(User.Identity.Name);
            ViewBag.UsersList = new List<UserModel>();
            return View();
        }

        //
        // POST: /Tasks/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                task.UserFromId = (from u in db.Users
                                   where u.User_login.Equals(User.Identity.Name)
                                   select u.User_id).Single();

                task.UserStatusChangedBy_id = task.UserFromId;

                Task newTask = new Task()
                {
                    Group_id = task.Group_id,
                    Task_creationDate = DateTime.Now,
                    Task_deadline = task.Deadline,
                    Task_description = task.Description,
                    Task_statusLastChange = DateTime.Now,
                    Task_title = task.Title,
                    TaskStatus_id = 1,
                    User_statusChangedBy = task.UserStatusChangedBy_id,
                    User_from = task.UserFromId,
                    User_to = task.UserTo_id
                };

                db.Tasks.Add(newTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        //
        // GET: /Tasks/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Task task = db.Tasks.SingleOrDefault(t => t.Task_id.Equals(id));
            if (task == null)
            {
                return HttpNotFound();
            }

            TaskModel tm = new TaskModel()
            {
                ToMe = task.User1.User_login.Equals(User.Identity.Name),
                CreationDate = task.Task_creationDate,
                Deadline = task.Task_deadline,
                Description = task.Task_description,
                Group_id = task.Group_id,
                Id = task.Task_id,
                StatusLastChange = task.Task_statusLastChange,
                TaskStatus_id = task.TaskStatus_id,
                TaskStatus_name = task.TaskStatu.TaskStatus_name,
                Title = task.Task_title,
                UserFromId = task.User_from,
                UserFrom_login = task.User.User_login,
                UserTo_id = task.User_to,
                UserTo_login = task.User1.User_login,
                UserStatusChangedBy_id = task.User_statusChangedBy,
                User_login_statusChangedBy = task.User2.User_login,
                Group_name = task.Group.Group_name
            };

            if (task.User1.User_login != User.Identity.Name && task.User.User_login != User.Identity.Name)
            {
                return RedirectToAction("InsufficientRights", "Error");
            }

            ViewBag.GroupsList = GetGroups(User.Identity.Name);
            ViewBag.UsersList = new List<UserModel>();

            return View(tm);
        }

        //
        // POST: /Tasks/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                Task editTask = db.Tasks.SingleOrDefault(t => t.Task_id.Equals(task.Id));

                if (task.Deadline != editTask.Task_deadline)
                    editTask.Task_deadline = task.Deadline;
                if (task.Description != editTask.Task_description)
                    editTask.Task_description = task.Description;
                if (task.Title != editTask.Task_title)
                    editTask.Task_title = task.Title;
                if (task.UserTo_id != editTask.User_to)
                    editTask.User_to = task.UserTo_id;
                if (task.Group_id != editTask.Group_id)
                    editTask.Group_id = task.Group_id;

                db.SaveChanges();
                return RedirectToAction("Details", new { id = task.Id });
            }
            return View(task);
        }

        //
        // GET: /Tasks/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            if (task.User1.User_login != User.Identity.Name && task.User.User_login != User.Identity.Name)
            {
                return RedirectToAction("InsufficientRights", "Error");
            }

            return View(task);
        }

        //
        // POST: /Tasks/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}