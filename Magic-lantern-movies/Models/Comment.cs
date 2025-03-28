using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using SQLite;

namespace Views
{
    // Class to represent a Comment
    public class Comment
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        // Constructor to create a new comment
        public Comment(string userName, string text)
        {
            UserName = userName;
            Text = text;
            DatePosted = DateTime.Now;
        }

        // Constructor to create a Comment from the database
        public Comment(int id, string userName, string text, DateTime datePosted)
        {
            Id = id;
            UserName = userName;
            Text = text;
            DatePosted = datePosted;
        }

        // Method to display the comment
        public void DisplayComment()
        {
            Console.WriteLine($"{UserName} (Posted on {DatePosted}): {Text}");
        }
    }

    // Class to represent a Movie with SQLite functionality
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }

        private readonly string _connectionString;

        // Constructor to create a new movie with SQLite connection string
        public Movie(string title, string connectionString)
        {
            Title = title;
            _connectionString = connectionString;
        }

        // Method to add a comment to the movie in the database
        public void AddComment(string userName, string commentText)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Insert the comment into the database
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

        // Method to display all comments of the movie from the database
        public void DisplayComments()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Query the comments from the database
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
                                reader.GetInt32(0),  // Id
                                reader.GetString(1), // UserName
                                reader.GetString(2), // Text
                                reader.GetDateTime(3) // DatePosted
                            );
                            comment.DisplayComment();
                        }
                    }
                }
            }
        }

        // Method to initialize the SQLite database (Create tables if they don't exist)
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

    // Main class to test the functionality
    class Program
    {
        static void Main(string[] args)
        {
            // SQLite connection string (stores database in the current directory)
            string connectionString = "Data Source=movie_comments.db;Version=3;";

            // Initialize the SQLite database with tables
            Movie.InitializeDatabase(connectionString);

            // Create a new movie instance
            Movie movie = new Movie("Inception", connectionString);

            // Add comments to the movie
            movie.AddComment("Alice", "Great movie, loved the concept!");
            movie.AddComment("Bob", "Amazing visuals, but the ending was confusing.");

            // Display all comments for the movie
            movie.DisplayComments();
        }
    }
}
