using Data;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
        public async Task InitializeMoviesAsync()
        {
            var existingMovies = await _databaseContext.GetMoviesAsync().ConfigureAwait(false);
            if (existingMovies.Any())
            {
                Debug.WriteLine($"Movies already exist");
                return; // Skip if movies already exist
            }
            var response = await _httpClient.GetAsync("https://www.imdb.com/chart/top/?ref_=tt_awd");

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string

                var content = await response.Content.ReadAsStringAsync();
                List<string> regexs = new() {
                    "\"name\":\"(.*?)\",",
                    "\"description\":\"(.*?)\",",
                    "\"image\":\"(.*?)\",",
                    "\"ratingValue\":(.*?),",
                    "\"contentRating\":\"(.*?)\",",
                    "\"genre\":\"(.*?)\",",
                    "\"duration\":\"(.*?)\"",

                };
                string moviePattern = "\"edges\":\\[(.*)\\]";
                string yearPatttern = "";
                string movieMatches = Regex.Match(content, moviePattern).Groups[1].Value;
                MatchCollection otherMatches = Regex.Matches(movieMatches, "{\"currentRank\":\\d+,\"node\":{(.*?)},\"__typename\":\"ChartTitleEdge\"},");
                var movies = new List<Movie>();
                foreach (Match match in otherMatches)
                {
                    Debug.WriteLine(match.Groups[0].Value);
                    Movie movie = new Movie();
                    movie.Name = Regex.Match(match.Value, "\"originalTitleText\":{\"text\":\"(.*?)\",").Groups[1].Value;
                    movie.Description = Regex.Match(match.Value, ".*?\"plotText\":{\"plainText\":\"(.*?)\",.*?}").Groups[1].Value;
                    movie.Image = Regex.Match(match.Value, "\"primaryImage\":{.*?\"url\":\"(.*?)\",.*?}").Groups[1].Value;
                    string year = Regex.Match(match.Value, "\"releaseYear\":{\"year\":(.*?),.*?},").Groups[1].Value;

                    movie.PublicationDate = DateTime.ParseExact(year, "yyyy", null);

                    //var rating = double.Parse(Regex.Match(match.Value, "\"ratingsSummary\":{\"aggregateRating\":.*?,}").Groups[1].Value.Replace(".",","));
                    //movie.Rating = rating > 8.0 ? Ratings.VeryGood : Ratings.Good;
                    movie.AgeRating = Regex.Match(match.Value, "\"certificate\":{\"rating\":(.*?),}").Groups[1].Value;
                    movie.Duration = TimeSpan.FromSeconds(int.Parse(Regex.Match(match.Value, "\"runtime\":{\"seconds\":(.*?),.*?},").Groups[1].Value));

                    movie.Categories = Regex.Match(match.Value, "\"genre\":\"(.*?)\",").Groups[1].Value.Split(", ").ToList();

                    movies.Add(movie);
                }
                foreach (var movie in movies)
                {
                    var result = await _databaseContext.SaveMovieAsync(movie).ConfigureAwait(false);
                    Debug.WriteLine($"Movie '{movie.Name}' saved with ID: {result}");
                }
                /*
                List<string> regexs = new() {
                    "\"name\":\"(.*?)\",",
                    "\"description\":\"(.*?)\",",
                    "\"image\":\"(.*?)\",",
                    "\"ratingValue\":(.*?),",
                    "\"contentRating\":\"(.*?)\",",
                    "\"genre\":\"(.*?)\",",
                    "\"duration\":\"(.*?)\"",

                };
                foreach(string pattern in regexs)
                {
                    MatchCollection matches = Regex.Matches(content, pattern);
                    foreach (Match match in matches)
                    {
                        Debug.WriteLine(match.Groups[1].Value);
                    }
                }
                */

                /*
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
                 */
                //Debug.WriteLine(content);
            }
            else
            {
                Debug.WriteLine("Error: " + response.StatusCode);
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
