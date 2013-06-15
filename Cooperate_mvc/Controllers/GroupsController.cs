using Cooperate_mvc.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Cooperate_mvc.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Groups/

        public ActionResult Index()
        {
            var groups = (from g in db.Groups
                          join p in db.Participations on g.Group_id equals p.Group_id
                          join u in db.Users on p.User_id equals u.User_id
                          where u.User_login.Equals(User.Identity.Name)
                          select new GroupModel()
                          {
                              Description = g.Group_description,
                              Name = g.Group_name,
                              Id = g.Group_id,
                              CreationDate = g.Group_creationDate,
                              IsAdmin = p.Participation_isAdmin
                          }).ToList();
            return View(groups);
        }

        //
        // GET: /Groups/Join/5

        public ActionResult Join(long id)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Group_id.Equals(id));
            if (group == null)
            {
                return HttpNotFound();
            }

            Participation p = new Participation() { Group_id = id, Participation_isAdmin = false };
            p.User_id = (from u in db.Users
                         where u.User_login.Equals(User.Identity.Name)
                         select u.User_id).Single();
            db.Participations.Add(p);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        //
        // GET: /Groups/Details/5

        public ActionResult Details(long id = 0)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Group_id.Equals(id));
            if (group == null)
            {
                return HttpNotFound();
            }

            Participation participation = group.Participations.SingleOrDefault(p => p.User.User_login.Equals(User.Identity.Name));

            ViewBag.IsParticipant = (participation != null);
            ViewBag.IsAdmin = (participation != null && participation.Participation_isAdmin);

            GroupModel groupModel = new GroupModel()
            {
                Description = group.Group_description,
                Name = group.Group_name,
                CreationDate = group.Group_creationDate,
                Id = group.Group_id
            };
            groupModel.Members = (from u in db.Users
                                  join p in db.Participations on u.User_id equals p.User_id
                                  where p.Group_id.Equals(id)
                                  select new UserModel()
                                  {
                                      Login = u.User_login,
                                      Id = u.User_id,
                                      FirstName = u.User_firstName,
                                      LastName = u.User_lastName
                                  }).ToList();

            return View(groupModel);
        }

        //
        // GET: /Groups/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Groups/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                Group newGroup = new Group()
                {
                    Group_creationDate = DateTime.Now,
                    Group_description = group.Description,
                    Group_name = group.Name
                };
                db.Groups.Add(newGroup);

                int userId = (from u in db.Users
                              where u.User_login.Equals(User.Identity.Name)
                              select u.User_id).Single();

                Participation participation = new Participation()
                {
                    User_id = userId,
                    Group = newGroup,
                    Participation_isAdmin = true
                };
                db.Participations.Add(participation);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /Groups/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Group_id.Equals(id));
            if (group == null)
            {
                return HttpNotFound();
            }

            Participation part = (from p in db.Participations
                                  join u in db.Users on p.User_id equals u.User_id
                                  where u.User_login.Equals(User.Identity.Name) && p.Group_id.Equals(id)
                                  select p).SingleOrDefault();
            if (part == null || !part.Participation_isAdmin)
            {
                return RedirectToAction("InsufficientRights", "Error");
            }

            GroupModel groupModel = new GroupModel() { Description = group.Group_description, Name = group.Group_name, CreationDate = group.Group_creationDate, Id = group.Group_id };
            return View(groupModel);
        }

        //
        // POST: /Groups/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupModel group)
        {
            if (ModelState.IsValid)
            {
                Group editGroup = db.Groups.SingleOrDefault(g => g.Group_id.Equals(group.Id));
                if (editGroup == null)
                {
                    return HttpNotFound();
                }

                if (group.Name != editGroup.Group_name)
                    editGroup.Group_name = group.Name;
                if (group.Description != editGroup.Group_description)
                    editGroup.Group_description = group.Description;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //
        // GET: /Groups/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Group_id.Equals(id));
            if (group == null)
            {
                return HttpNotFound();
            }

            Participation part = (from p in db.Participations
                                  join u in db.Users on p.User_id equals u.User_id
                                  where u.User_login.Equals(User.Identity.Name) && p.Group_id.Equals(id)
                                  select p).SingleOrDefault();
            if (part == null || !part.Participation_isAdmin)
            {
                return RedirectToAction("InsufficientRights", "Error");
            }

            GroupModel groupModel = new GroupModel()
            {
                CreationDate = group.Group_creationDate,
                Name = group.Group_name,
                Description = group.Group_description
            };
            return View(groupModel);
        }

        //
        // POST: /Groups/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Group group = db.Groups.SingleOrDefault(g => g.Group_id.Equals(id));
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