using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using halpert.Models;
using halpert.ViewModels;

namespace halpert.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext db;
        public AccountsController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            bool IsValidUser = db.Users.Any(u => u.Email.ToLower() ==
                     user.Email.ToLower() && u.Password == user.Password);

            if (IsValidUser)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("RightPage");
            }

            ModelState.AddModelError("", "invalid Username or Password");
            return View("Login");
        }

        public ActionResult RightPage()
        {
            if (User.IsInRole(RoleName.Manager))
                return RedirectToAction("Index", "Manager");
            else
                return RedirectToAction("Index", "Developer");
        }

        public ActionResult Register()
        {
            var viewModel = new RegisterVM
            {
                Roles = db.Roles.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Register(RegisterVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }

            var user = new User
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Password = viewModel.Password
            };

            if (db.Users.Any(u => u.Email == user.Email))
            {
                var vm = new RegisterVM
                {
                    Roles = db.Roles.ToList()
                };

                ModelState.AddModelError("", "This email refer to another user!");
                return View("Register", vm);
            }

            db.Users.Add(user);

            // add user to his role;
            db.UserRolesMapping.Add(new UserRolesMapping
            {
                UserId = user.Id,
                RoleId = viewModel.RoleId
            });

            db.SaveChanges();
            
            return RedirectToAction("Login", "Accounts");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}