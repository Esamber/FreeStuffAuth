using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddPostsAuth.Data
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime DatePosted { get; set; }
        public string PhoneNumber { get; set; }
        public string Details { get; set; }
    }
}
