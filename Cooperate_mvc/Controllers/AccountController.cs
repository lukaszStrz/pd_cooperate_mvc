using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooperate_mvc.Models;
using System.Web.Security;

namespace Cooperate_mvc.Controllers
{
    public class AccountController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Account/Details/login

        public ActionResult Details(string id = "")
        {
            User user = db.Users.Where(u => u.User_login.Equals(id)).SingleOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /Account/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Account/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.User_pass = Hash.GetHash(user.User_pass, HashType.SHA512);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Account/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Account/Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, bool persistent)
        {
            MyMembership membership = new MyMembership();
            if (membership.ValidateUser(user.User_login, user.User_pass))
            {
                FormsAuthentication.SetAuthCookie(user.User_login, persistent);
                return RedirectToAction("Index", "Home");
            }
            return Login();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}