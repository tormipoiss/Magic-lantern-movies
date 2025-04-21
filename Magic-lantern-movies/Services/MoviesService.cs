using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using True_Comment;

namespace Services
{
    public class MoviesService
    {
        private readonly DatabaseContext _databaseContext;

        public MoviesService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task InitializeMoviesAsync()
        {
            try
            {
                var existingMovies = await _databaseContext.GetMoviesAsync().ConfigureAwait(false);
                if (existingMovies.Any())
                {
                    Debug.WriteLine($"Movies already exist");
                    return;
                }

                var movies = new List<Models.Movie>
                {
                    new()
                    {
                        Name = "The Shawshank Redemption",
                        Description = "A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict, while maintaining his innocence and trying to remain hopeful through simple compassion.",
                        Rating = Ratings.VeryGood,
                        Image = "https://upload.wikimedia.org/wikipedia/en/8/81/ShawshankRedemptionMoviePoster.jpg",
                        Categories = new() { Categories.Drama },
                        Actors = new() { "Tim Robbins", "Morgan Freeman", "Bob Gunton" },
                        PublicationDate = new DateTime(1994, 9, 10),
                        OriginalLanguage = "English",
                        Director = "Frank Darabont",
                        Duration = new TimeSpan(2, 22, 0),
                        AgeRating = AgeRatings.R,
                    },

                    new()
                    {
                        Name = "The Godfather",
                        Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                        Rating = Ratings.VeryGood,
                        Image = "https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg",
                        Categories = new() { Categories.Drama, Categories.Tragedy, Categories.Crime },
                        Actors = new() { "Marlon Brando", "Al Pacino", "James Caan" },
                        PublicationDate = new DateTime(1972, 3, 14),
                        OriginalLanguage = "English",
                        Director = "Francis Ford Coppola",
                        Duration = new TimeSpan(2, 55, 0),
                        AgeRating = AgeRatings.R,
                    }
                };

                foreach (var movie in movies)
                {
                    var result = await _databaseContext.SaveMovieAsync(movie).ConfigureAwait(false);
                    Debug.WriteLine($"Movie '{movie.Name}' saved with ID: {result}");

                    await AddSampleCommentsForMovieAsync(movie.Name);
                }

                await DisplayMoviesWithCommentsAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing movies: {ex.Message}");
            }
        }

        private async Task AddSampleCommentsForMovieAsync(string movieName)
        {
            var comment1 = new Comment("Alice", "Amazing movie, loved the depth of the characters!");
            var comment2 = new Comment("Bob", "Great story, but the pacing was a bit slow.");

            var result1 = await _databaseContext.SaveMovieAsync(new Movie
            {
                Name = movieName, 
                Description = comment1.Text,
            });

            Debug.WriteLine($"Added comment 1 for '{movieName}'");

            var result2 = await _databaseContext.SaveMovieAsync(new Movie
            {
                Name = movieName, 
                Description = comment2.Text,
            });
            Debug.WriteLine($"Added comment 2 for '{movieName}'");
        }

        private async Task DisplayMoviesWithCommentsAsync()
        {
            try
            {
                var movies = await _databaseContext.GetMoviesAsync().ConfigureAwait(false);
                foreach (var movie in movies)
                {
                    Console.WriteLine($"Movie: {movie.Name} - {movie.Description}");
                    Console.WriteLine($"Rating: {movie.Rating}");
                    Console.WriteLine($"Categories: {string.Join(", ", movie.Categories)}");
                    Console.WriteLine($"Actors: {string.Join(", ", movie.Actors)}");
                    Console.WriteLine($"Released: {movie.PublicationDate.ToString("MMMM dd, yyyy")}");
                    Console.WriteLine($"Language: {movie.OriginalLanguage}");
                    Console.WriteLine($"Duration: {movie.Duration.Hours} hours {movie.Duration.Minutes} minutes");
                    Console.WriteLine($"Age Rating: {movie.AgeRating}");

                    Console.WriteLine("Comments:");
                    var comments = movie.Comments;
                    if (comments != null && comments.Any())
                    {
                        foreach (var comment in comments)
                        {
                            comment.DisplayComment();
                        }
                    }
                    else
                    {
                        Console.WriteLine("No comments available.");
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error displaying movies with comments: {ex.Message}");
            }
        }
    }
}
