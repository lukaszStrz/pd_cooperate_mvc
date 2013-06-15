﻿using Cooperate_mvc.Models;
using HashLib;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cooperate_mvc.Controllers
{
    public class AccountController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Account/Details/login

        [Authorize]
        public ActionResult Details(string login = "")
        {
            User user = db.Users.SingleOrDefault(u => u.User_login.Equals(login));
            if (user == null)
            {
                return HttpNotFound();
            }
            UserModel userModel = new UserModel()
            {
                Birth = user.User_birth,
                Email = user.User_email,
                FirstName = user.User_firstName,
                LastName = user.User_lastName,
                Login = user.User_login
            };
            return View(userModel);
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
        public ActionResult Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                User newUser = new Models.User();
                newUser.User_birth = user.Birth;
                newUser.User_email = user.Email;
                newUser.User_firstName = user.FirstName;
                newUser.User_lastName = user.LastName;
                newUser.User_login = user.Login;
                newUser.User_pass = Hash.GetHash(user.Pass, HashType.SHA512);

                db.Users.Add(newUser);
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(user.Login, false);

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        //
        // GET: /Account/Edit/login

        [Authorize]
        public ActionResult Edit(string login = "")
        {
            User user = db.Users.SingleOrDefault(u => u.User_login.Equals(login));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Account/Edit/

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                User u2 = db.Users.SingleOrDefault(u => u.User_login.Equals(User.Identity.Name));
                if (u2 == null)
                    return HttpNotFound();

                if (u2.User_firstName != user.User_firstName)
                    u2.User_firstName = user.User_firstName;
                if (u2.User_lastName != user.User_lastName)
                    u2.User_lastName = user.User_lastName;
                if (u2.User_birth != user.User_birth)
                    u2.User_birth = user.User_birth;

                db.Entry(u2).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details", new { login = u2.User_login });
            }
            return View(user);
        }

        [HttpPost]
        public JsonResult LoginExist(string Login)
        {
            var user = db.Users.SingleOrDefault(u => u.User_login.Equals(Login));

            return Json(user == null);
        }

        [HttpPost]
        public JsonResult MailExist(string Email)
        {
            var user = db.Users.SingleOrDefault(u => u.User_email.Equals(Email));

            return Json(user == null);
        }

        //
        // GET: /Account/Login

        public ActionResult Login(bool error = false)
        {
            ViewBag.ValidationError = error;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, bool persistent)
        {
            if (ValidateUser(user.User_login, user.User_pass, persistent))
            {
                var returnUrl = HttpUtility.HtmlDecode(Request.QueryString["ReturnUrl"]);
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
            return Login(true);
        }

        [Authorize]
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

        private bool ValidateUser(string login, string password, bool persistent)
        {
            User user = (from u in db.Users
                         where u.User_login.Equals(login)
                         select u).SingleOrDefault();

            if (user != null && Hash.CheckHash(password, user.User_pass, HashType.SHA512))
            {
                FormsAuthentication.SetAuthCookie(login, persistent);
                return true;
            }
            return false;
        }
    }
}