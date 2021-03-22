using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using AddPostsAuth.Data;

namespace AddPostsAuth.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=AddPostsAuth;Integrated Security=true;";
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitAccount(Account a)
        {
            PostsDb db = new(_connectionString);
            db.AddUser(a);
            //return Redirect($"/account/submitlogin?email={a.Email}&password={a.Password}");
            return Redirect("/account/login");
        }
        public IActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult SubmitLogin(string email, string password)
        {
            PostsDb db = new(_connectionString);
            User user = db.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Incorrect Email/Password Combo, Please try again!";
                return Redirect("/account/login");
            }
                var claims = new List<Claim>
            {
                new Claim("user", email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
