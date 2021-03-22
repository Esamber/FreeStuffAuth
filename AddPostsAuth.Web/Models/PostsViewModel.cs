using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddPostsAuth.Data;

namespace AddPostsAuth.Web.Models
{
    public class PostsViewModel
    {
        public List<Post> Posts { get; set; }
        public List<int> UserSubmittedIds { get; set; }
    }
    public class AddPostViewModel
    {
        public int currentUserId { get; set; }
    }
    public class MyPostsViewModel
    {
        public List<Post> Posts { get; set; }
    }
}
