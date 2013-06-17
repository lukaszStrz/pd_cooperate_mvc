using Cooperate_mvc.Models;
using System;
using System.Collections.Generic;
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

        private List<PostModel> GetPosts(long groupId)
        {
            return (from p in db.Posts
                    where p.Group_id.Equals(groupId) && p.Post_inResponseTo == null
                    orderby p.Post_datetime descending
                    select new PostModel()
                    {
                        Author_login = p.User.User_login,
                        Datetime = p.Post_datetime,
                        Id = p.Post_id,
                        Text = p.Post_text,
                    }).ToList();
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

            ViewBag.Posts = GetPosts(id);

            return View(groupModel);
        }

        [HttpPost]
        public PartialViewResult AddPost(string PostText, long? Id)
        {
            if (!Request.IsAjaxRequest() || PostText == null || Id == null)
                return null;

            Post post = new Post()
            {
                Group_id = (long)Id,
                Post_datetime = DateTime.Now,
                Post_text = PostText,
                User_author = (from u in db.Users
                               where u.User_login.Equals(User.Identity.Name)
                               select u.User_id).Single()
            };
            db.Posts.Add(post);
            db.SaveChanges();

            var postModel = new PostModel()
            {
                Author_login = User.Identity.Name,
                Text = PostText,
                Datetime = post.Post_datetime,
                Id = post.Post_id
            };

            var users = (from u in db.Users
                         join p in db.Participations on u.User_id equals p.User_id
                         join g in db.Groups on p.Group_id equals g.Group_id
                         where g.Group_id.Equals((long)Id) && !u.User_login.Equals(User.Identity.Name)
                         select u.User_login).ToList();

            string message = this.PartialViewToString("_Post", postModel);

            foreach (var login in users)
            {
                GroupsSocket.SendTo(login, message);
            }

            return PartialView("_Post", postModel); ;
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

            GroupModel groupModel = new GroupModel()
            {
                Description = group.Group_description,
                Name = group.Group_name,
                CreationDate = group.Group_creationDate,
                Id = group.Group_id
            };
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
                return RedirectToAction("Details", new { id = group.Id });
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