﻿using Cooperate_mvc.Models;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Cooperate_mvc.Controllers
{
    public class GroupsController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Groups/

        [Authorize]
        public ActionResult Index()
        {
            var groups = (from g in db.Groups
                          join p in db.Participations on g.Group_id equals p.Group_id
                          join u in db.Users on p.User_id equals u.User_id
                          where u.User_login.Equals(User.Identity.Name)
                          select new GroupModel() { Description = g.Group_description, Name = g.Group_name, Id = g.Group_id, CreationDate = g.Group_creationDate });
            return View(groups.ToList());
        }

        //
        // GET: /Groups/Details/5

        [Authorize]
        public ActionResult Details(long id = 0)
        {
            Group group = db.Groups.Where(g => g.Group_id.Equals(id)).SingleOrDefault();
            if (group == null)
            {
                return HttpNotFound();
            }
            GroupModel groupModel = new GroupModel() { Description = group.Group_description, Name = group.Group_name, CreationDate = group.Group_creationDate, Id = group.Group_id };
            return View(groupModel);
        }

        //
        // GET: /Groups/Create

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Groups/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                Group newGroup = new Group() { Group_creationDate = DateTime.Now, Group_description = group.Description, Group_name = group.Name };
                db.Groups.Add(newGroup);

                int userId = (from u in db.Users
                              where u.User_login.Equals(User.Identity.Name)
                              select u.User_id).Single();

                Participation participation = new Participation() { User_id = userId, Group = newGroup, Participation_isAdmin = true };
                db.Participations.Add(participation);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /Groups/Edit/5

        [Authorize]
        public ActionResult Edit(long id = 0)
        {
            Group group = db.Groups.Where(g => g.Group_id.Equals(id)).SingleOrDefault();
            if (group == null)
            {
                return HttpNotFound();
            }
            GroupModel groupModel = new GroupModel() { Description = group.Group_description, Name = group.Group_name, CreationDate = group.Group_creationDate, Id = group.Group_id };
            return View(groupModel);
        }

        //
        // POST: /Groups/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                Group editGroup = db.Groups.Where(g => g.Group_id.Equals(group.Id)).SingleOrDefault();
                if (editGroup == null)
                {
                    return HttpNotFound();
                }
                editGroup.Group_name = group.Name;
                editGroup.Group_description = group.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //
        // GET: /Groups/Delete/5

        [Authorize]
        public ActionResult Delete(long id = 0)
        {
            Group group = db.Groups.Where(g => g.Group_id.Equals(id)).SingleOrDefault();
            if (group == null)
            {
                return HttpNotFound();
            }
            GroupModel groupModel = new GroupModel() { CreationDate = group.Group_creationDate, Name = group.Group_name, Description = group.Group_description };
            return View(groupModel);
        }

        //
        // POST: /Groups/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(long id)
        {
            Group group = db.Groups.Where(g => g.Group_id.Equals(id)).SingleOrDefault();
            db.Groups.Remove(group);
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