using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace AddPostsAuth.Data
{
    public class PostsDb
    {
        private readonly string _connectionString;
        public PostsDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Post> GetPosts()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Posts";
            connection.Open();
            List<Post> posts = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                posts.Add(new Post
                {
                    Name = (string)reader["Name"],
                    DatePosted = (DateTime)reader["DatePosted"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"],
                    Id = (int)reader["Id"]
                });
            }
            return posts.OrderByDescending(p => p.Id).ToList();
        }
        public List<Post> GetPosts(int userId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Posts WHERE UserId = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            List<Post> posts = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                posts.Add(new Post
                {
                    Name = (string)reader["Name"],
                    DatePosted = (DateTime)reader["DatePosted"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Details = (string)reader["Details"],
                    Id = (int)reader["Id"]
                });
            }
            return posts.OrderByDescending(p => p.Id).ToList();
        }
        public User Login(string email, string password)
        {
            User user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;
        }
        public void AddUser(Account a)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (FirstName, LastName, Email, PasswordHash) VALUES (@firstName, @lastName, @email, @passwordHash)";
            cmd.Parameters.AddWithValue("@firstName", a.FirstName);
            cmd.Parameters.AddWithValue("@lastName", a.LastName);
            cmd.Parameters.AddWithValue("@email", a.Email);
            cmd.Parameters.AddWithValue("@passwordHash", BCrypt.Net.BCrypt.HashPassword(a.Password));
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public void AddPost(Post p)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Posts (Name, UserId, DatePosted, PhoneNumber, Details) VALUES (@name, @userId, @datePosted, @phoneNumber, @details) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@userId", p.UserId);
            cmd.Parameters.AddWithValue("@datePosted", DateTime.Now);
            cmd.Parameters.AddWithValue("@phoneNumber", p.PhoneNumber);
            cmd.Parameters.AddWithValue("@details", p.Details);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
        public void DeletePost(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Posts WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Email = (string)reader["Email"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }
        public List<int> GetUserSubmittedIds(int userId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Posts WHERE UserId = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            List<int> userSubmittedIds = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                userSubmittedIds.Add((int)reader["Id"]);
            }
            return userSubmittedIds;
        }
    }
}
