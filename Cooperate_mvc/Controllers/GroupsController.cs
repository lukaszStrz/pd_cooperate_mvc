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
                          select g);
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
            return View(group);
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
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                group.Group_creationDate = DateTime.Now;
                db.Groups.Add(group);

                int userId = (from u in db.Users
                              where u.User_login.Equals(User.Identity.Name)
                              select u.User_id).Single();

                Participation participation = new Participation();
                participation.User_id = userId;
                participation.Group = group;
                participation.Participation_isAdmin = true;
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
            return View(group);
        }

        //
        // POST: /Groups/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
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
            return View(group);
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