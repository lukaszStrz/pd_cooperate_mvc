﻿using System;
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
                             Task_statusLastChange = t.Task_statusLastChange,
                             TaskStatus_id = t.TaskStatus_id,
                             TaskStatus=ts.TaskStatus_name,
                             Title = t.Task_title,
                             User_from = t.User_from,
                             User_login_from = uFrom.User_login,
                             User_to = t.User_to,
                             User_login_to = uTo.User_login,
                             User_statusChangedBy = t.User_statusChangedBy,
                             User_login_statusChangedBy=uChanged.User_login,
                             Group=g.Group_name
                         });

            return View(tasks.ToList());
        }

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
            ViewBag.Group_id = new SelectList(db.Groups, "Group_id", "Group_name");
            ViewBag.TaskStatus_id = new SelectList(db.TaskStatus, "TaskStatus_id", "TaskStatus_name");
            ViewBag.User_from = new SelectList(db.Users, "User_id", "User_firstName");
            ViewBag.User_to = new SelectList(db.Users, "User_id", "User_firstName");
            ViewBag.User_statusChangedBy = new SelectList(db.Users, "User_id", "User_firstName");
            return View();
        }

        //
        // POST: /Tasks/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
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