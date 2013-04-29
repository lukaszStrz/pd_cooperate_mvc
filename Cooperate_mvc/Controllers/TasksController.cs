using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooperate_mvc.Models;

namespace Cooperate_mvc.Controllers
{
    public class TasksController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Tasks/

        //public ActionResult Index()
        //{
        //    var tasks = (from t in db.Tasks
        //                 join uFrom in db.Users on t.User_from equals uFrom.User_id
        //                 join uTo in db.Users on t.User_to equals uTo.User_id
        //                 join uChanged in db.Users on t.User_statusChangedBy equals uChanged.User_id
        //                 join ts in db.TaskStatus on t.TaskStatus_id equals ts.TaskStatus_id
        //                 join g in db.Groups on t.Group_id equals g.Group_id
        //                 where uFrom.User_login.Equals(User.Identity.Name) || uTo.User_login.Equals(User.Identity.Name)
        //                 select new TaskModel()
        //                 {
        //                     ToMe = uTo.User_login.Equals(User.Identity.Name),
        //                     CreationDate = t.Task_creationDate,
        //                     Deadline = t.Task_deadline,
        //                     Description = t.Task_description,
        //                     Group_id = t.Group_id,
        //                     Id = t.Task_id,
        //                     StatusLastChange = t.Task_statusLastChange,
        //                     TaskStatus_id = t.TaskStatus_id,
        //                     TaskStatus_name = ts.TaskStatus_name,
        //                     Title = t.Task_title,
        //                     UserFromId = t.User_from,
        //                     UserFrom_login = uFrom.User_login,
        //                     UserTo_id = t.User_to,
        //                     UserTo_login = uTo.User_login,
        //                     UserStatusChangedBy_id = t.User_statusChangedBy,
        //                     User_login_statusChangedBy = uChanged.User_login,
        //                     Group_name = g.Group_name
        //                 });

        //    return View(tasks.ToList());
        //}

        //
        // GET: /Tasks/Details/5

        public ActionResult Details(long id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // GET: /Tasks/Create

        public ActionResult Create()
        {
            CreateTaskModel model = new CreateTaskModel()
            {
                Task = new TaskModel(),
                Users = null
            };

            model.Groups = (from g in db.Groups
                            join p in db.Participations on g.Group_id equals p.Group_id
                            join u in db.Users on p.User_id equals u.User_id
                            where u.User_login.Equals(User.Identity.Name)
                            select new GroupModel()
                            {
                                IsAdmin = p.Participation_isAdmin,
                                CreationDate = g.Group_creationDate,
                                Description = g.Group_description,
                                Id = g.Group_id,
                                Name = g.Group_name
                            }).ToList();

            return View(model);
        }

        //
        // POST: /Tasks/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTaskModel task)
        {
            if (ModelState.IsValid)
            {
                //task.UserFromId = (from u in db.Users
                //                  where u.User_login.Equals(User.Identity.Name)
                //                  select u.User_id).Single();

                //task.UserStatusChangedBy_id = task.UserFromId;

                //Task newTask = new Task()
                //{
                //    Group_id = task.Group_id,
                //    Task_creationDate = DateTime.Now,
                //    Task_deadline = task.Deadline,
                //    Task_description = task.Description,
                //    Task_statusLastChange = DateTime.Now,
                //    Task_title = task.Title,
                //    TaskStatus_id = task.TaskStatus_id,
                //    User_statusChangedBy = task.UserStatusChangedBy_id,
                //    User_from = task.UserFromId,
                //    User_to = task.UserTo_id
                //};

                //db.Tasks.Add(newTask);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(task);
        }

        [HttpPost]
        public ActionResult SelectGroup(long SelectedGroupId)
        {
            CreateTaskModel model = new CreateTaskModel();
            model.Users = (from u in db.Users
                           join p in db.Participations on u.User_id equals p.User_id
                           join g in db.Groups on p.Group_id equals g.Group_id
                           where g.Group_id.Equals(SelectedGroupId)
                           select new UserModel()
                           {
                               Birth = u.User_birth,
                               Email = u.User_email,
                               FirstName = u.User_firstName,
                               LastName = u.User_lastName,
                               Login = u.User_login
                           }).ToList();

            return PartialView("_SelectUserTo", model);
        }

        //
        // GET: /Tasks/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.Group_id = new SelectList(db.Groups, "Group_id", "Group_name", task.Group_id);
            ViewBag.TaskStatus_id = new SelectList(db.TaskStatus, "TaskStatus_id", "TaskStatus_name", task.TaskStatus_id);
            ViewBag.User_from = new SelectList(db.Users, "User_id", "User_firstName", task.User_from);
            ViewBag.User_to = new SelectList(db.Users, "User_id", "User_firstName", task.User_to);
            ViewBag.User_statusChangedBy = new SelectList(db.Users, "User_id", "User_firstName", task.User_statusChangedBy);
            return View(task);
        }

        //
        // POST: /Tasks/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Group_id = new SelectList(db.Groups, "Group_id", "Group_name", task.Group_id);
            ViewBag.TaskStatus_id = new SelectList(db.TaskStatus, "TaskStatus_id", "TaskStatus_name", task.TaskStatus_id);
            ViewBag.User_from = new SelectList(db.Users, "User_id", "User_firstName", task.User_from);
            ViewBag.User_to = new SelectList(db.Users, "User_id", "User_firstName", task.User_to);
            ViewBag.User_statusChangedBy = new SelectList(db.Users, "User_id", "User_firstName", task.User_statusChangedBy);
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