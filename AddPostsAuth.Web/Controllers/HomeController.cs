using AddPostsAuth.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AddPostsAuth.Data;
using Microsoft.AspNetCore.Authorization;

namespace AddPostsAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=AddPostsAuth;Integrated Security=true;";

        public IActionResult Index()
        {
            PostsDb db = new(_connectionString);
            PostsViewModel vm = new()
            {
                Posts = db.GetPosts()
            };
            if (User.Identity.IsAuthenticated)
            {
                User currentUser = db.GetByEmail(User.Identity.Name);
                vm.UserSubmittedIds = db.GetUserSubmittedIds(currentUser.Id);
            }
            else
            {
                vm.UserSubmittedIds = new List<int>();
            }
            return View(vm);
        }
        [Authorize]
        public IActionResult AddPost()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/account/login");
            }
            PostsDb db = new(_connectionString);
            User currentUser = db.GetByEmail(User.Identity.Name);
            AddPostViewModel vm = new()
            {
                currentUserId = currentUser.Id
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult SubmitPost(Post p)
        {
            PostsDb db = new(_connectionString);
            db.AddPost(p);
            return Redirect("/home/index");
        }
        public IActionResult DeletePost(int postId)
        {
            PostsDb db = new(_connectionString);
            db.DeletePost(postId);
            return Redirect("/home/index");
        }
        public IActionResult MyPosts()
        {
            PostsDb db = new(_connectionString);
            User currentUser = db.GetByEmail(User.Identity.Name);
            MyPostsViewModel vm = new()
            {
                Posts = db.GetPosts(currentUser.Id)
            };
            return View(vm);
        }
    }
}
