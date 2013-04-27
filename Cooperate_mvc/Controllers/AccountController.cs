using Cooperate_mvc.Models;
using System.Linq;
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
        public ActionResult Create(UserAccount user)
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

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public JsonResult LoginExist(string Login)
        {
            var user = db.Users.Where(u => u.User_login.Equals(Login)).SingleOrDefault();

            return Json(user == null);
        }

        [HttpPost]
        public JsonResult MailExist(string Email)
        {
            var user = db.Users.Where(u => u.User_email.Equals(Email)).SingleOrDefault();

            return Json(user == null);
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
    }
}