using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using SQLite;
using SQLiteCommand = System.Data.SQLite.SQLiteCommand;
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;

namespace Views
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        public Comment(string userName, string text)
        {
            UserName = userName;
            Text = text;
            DatePosted = DateTime.Now;
        }

        public Comment(int id, string userName, string text, DateTime datePosted)
        {
            Id = id;
            UserName = userName;
            Text = text;
            DatePosted = datePosted;
        }

        public void DisplayComment()
        {
            Console.WriteLine($"{UserName} (Posted on {DatePosted}): {Text}");
        }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }

        private readonly string _connectionString;

        public Movie(string title, string connectionString)
        {
            Title = title;
            _connectionString = connectionString;
        }

        public void AddComment(string userName, string commentText)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Comments (MovieTitle, UserName, Text, DatePosted) VALUES (@MovieTitle, @UserName, @Text, @DatePosted)";
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MovieTitle", Title);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@Text", commentText);
                    cmd.Parameters.AddWithValue("@DatePosted", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisplayComments()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Comments WHERE MovieTitle = @MovieTitle ORDER BY DatePosted DESC";
                using (var cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MovieTitle", Title);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No comments yet.");
                            return;
                        }

                        while (reader.Read())
                        {
                            var comment = new Comment(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDateTime(3)
                            );
                            comment.DisplayComment();
                        }
                    }
                }
            }
        }


        public static void InitializeDatabase(string connectionString)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createMoviesTable = @"CREATE TABLE IF NOT EXISTS Movies (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Title TEXT NOT NULL)";

                string createCommentsTable = @"CREATE TABLE IF NOT EXISTS Comments (
                                               Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                               MovieTitle TEXT NOT NULL,
                                               UserName TEXT NOT NULL,
                                               Text TEXT NOT NULL,
                                               DatePosted DATETIME NOT NULL)";

                using (var cmd = new SQLiteCommand(createMoviesTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SQLiteCommand(createCommentsTable, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Data Source=movie_comments.db;Version=3;";


            Movie.InitializeDatabase(connectionString);


            Movie movie = new Movie("Inception", connectionString);

            movie.AddComment("Alice", "Great movie, loved the concept!");
            movie.AddComment("Bob", "Amazing visuals, but the ending was confusing.");

            movie.DisplayComments();
        }
    }
}
