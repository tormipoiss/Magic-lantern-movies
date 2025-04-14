using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Services
{
    public class MoviesService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly HttpClient _httpClient;

        public MoviesService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _httpClient = new HttpClient();
        }
        private class MovieObj
        {
            public bool Adult { get; set; }
            public string Backdrop_path { get; set; }
            public List<int> Genre_ids { get; set; }
            public int Id { get; set; }
            public string Original_language { get; set; }
            public string Original_Title { get; set; }
            public string Overview { get; set; }
            public float Popularity { get; set; }
            public string Poster_path { get; set; }
            public string Release_date { get; set; }
            public string Title { get; set; }
            public bool Video { get; set; }
            public float Vote_average { get; set; }
            public int Vote_count { get; set; }
        }
        private class PopularMoviesJson
        {
            public int Page { get; set; }
            public List<MovieObj> Results { get; set; }
        }
        private async Task<Ratings> ParseRating(float ratingInput)
        {
            int rating = (int)ratingInput;
            if (rating == 1) return Ratings.VeryNegative;
            if (rating == 2 || rating == 3) return Ratings.Negative;
            if (rating == 4 || rating == 5) return Ratings.Neutral;
            if (rating == 6 || rating == 7) return Ratings.Good;
            if (rating >= 8 && rating <= 10) return Ratings.VeryGood;
            return Ratings.Neutral;

        }

        public async Task InitializeMoviesAsync()
        {
            var existingMovies = await _databaseContext.GetMoviesAsync().ConfigureAwait(false);
            if (existingMovies.Any())
            {
                Debug.WriteLine($"Movies already exist");
                return; // Skip if movies already exist
            }
            var movies = new List<Movie>();
            const string api_key = "91e0d4296bdfc99f07241e1b39b1f41f";
            for(int i = 0; i <= 2; i++)
            {
                var popularMoviesResp = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/popular?page={i}&api_key=" + api_key);
                if (popularMoviesResp.IsSuccessStatusCode)
                {
                    var content = await popularMoviesResp.Content.ReadAsStringAsync();
                    PopularMoviesJson data = JsonConvert.DeserializeObject<PopularMoviesJson>(content);
                    foreach (MovieObj movieObj in data.Results)
                    {
                        var movie = new Movie();
                        var movieDetailsResp = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/{movieObj.Id}?api_key=" + api_key);
                        var detailsContent = await movieDetailsResp.Content.ReadAsStringAsync();
                        JObject detailsData = JObject.Parse(detailsContent);
                        movie.Categories = detailsData["genres"].Select(g => g["name"].ToString()).ToList();
                        movie.Duration = TimeSpan.FromMinutes(detailsData["runtime"].ToObject<int>());
                        movie.Name = movieObj.Title;
                        movie.Description = movieObj.Overview;
                        movie.Image = "https://image.tmdb.org/t/p/w500" + movieObj.Poster_path;
                        movie.BackImage = "https://image.tmdb.org/t/p/w500" + movieObj.Backdrop_path;
                        movie.OriginalLanguage = movieObj.Original_language;
                        movie.Rating = await ParseRating(movieObj.Vote_average);
                        movie.PublicationDate = DateTime.Parse(movieObj.Release_date.Replace("-", "/"));
                        var result = await _databaseContext.SaveMovieAsync(movie);
                        Debug.WriteLine($"Movie '{movie.Name}' saved with ID: {result}");
                        await Task.Delay(200);

                    }
                }
            }
            /*
            try
            {
                var existingMovies = await _databaseContext.GetMoviesAsync().ConfigureAwait(false);
                if (existingMovies.Any()) 
                {
                    Debug.WriteLine($"Movies already exist");
                    return; // Skip if movies already exist
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
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing movies: {ex.Message}");
            }
            */
        }
    }
}
