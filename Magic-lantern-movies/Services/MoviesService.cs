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
                        Directors = new() { "Frank Darabont" },
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
                        Directors = new() { "Francis Ford Coppola" },
                        Duration = new TimeSpan(2, 55, 0),
                        AgeRating = AgeRatings.R,
                    },
                    new()
                    {
                        Name = "The Dark Knight",
                        Description = "When a menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end to the madness.",
                        Rating = Ratings.VeryGood,
                        Image = "https://upload.wikimedia.org/wikipedia/en/1/1c/The_Dark_Knight_%282008_film%29.jpg",
                        Categories = new() { Categories.Action, Categories.Tragedy, Categories.Superhero, Categories.Epic },
                        Actors = new() { "Christian Bale", "Heath Ledger", "Aaron Eckhart" },
                        PublicationDate = new DateTime(2008, 7, 18),
                        OriginalLanguage = "English",
                        Directors = new() { "Christopher Nolan" },
                        Duration = new TimeSpan(2, 32, 0),
                        AgeRating = AgeRatings.PG_13,
                    },
                    new()
                    {
                        Name = "Disaster Movie",
                        Description = "Over the course of one evening, an unsuspecting group of twenty-somethings find themselves bombarded by a series of natural disasters and catastrophic events.",
                        Rating = Ratings.VeryNegative,
                        Image = "https://upload.wikimedia.org/wikipedia/en/a/af/Disaster_movie.jpg",
                        Categories = new() { Categories.Disaster, Categories.Parody, Categories.Superhero },
                        Actors = new() { "Carmen Electra", "Vanessa Lachey", "Nichole Parker" },
                        PublicationDate = new DateTime(2008, 8, 29),
                        OriginalLanguage = "English",
                        Directors = new() { "Jason Friedberg", "Aaron Seltzer" },
                        Duration = new TimeSpan(1, 27, 0),
                        AgeRating = AgeRatings.PG_13,
                    },
                    new()
                    {
                        Name = "Epic Movie",
                        Description = "A spoof on previous years' epic movies (The Da Vinci Code (2006), The Chronicles of Narnia: The Lion, the Witch and the Wardrobe (2005) + 20 more), TV series, music videos and celebs. 4 orphans are on an epic adventure.",
                        Rating = Ratings.Negative,
                        Image = "https://upload.wikimedia.org/wikipedia/en/0/02/Epicmovieposter.jpg",
                        Categories = new() { Categories.Adventure, Categories.Parody, Categories.Comedy },
                        Actors = new() { "Kal Penn", "Jennifer Coolidge", "Fred Willard" },
                        PublicationDate = new DateTime(2007, 1, 26),
                        OriginalLanguage = "English",
                        Directors = new() { "Jason Friedberg", "Aaron Seltzer" },
                        Duration = new TimeSpan(1, 26, 0),
                        AgeRating = AgeRatings.PG_13,
                    },
                    new()
                    {
                        Name = "A Minecraft Movie",
                        Description = "Four misfits are suddenly pulled through a mysterious portal into a bizarre, cubic wonderland that thrives on imagination. To get back home, they'll have to master this world while embarking on a quest with an unexpected, expert crafter.",
                        Rating = Ratings.Good,
                        Image = "https://upload.wikimedia.org/wikipedia/en/6/66/A_Minecraft_Movie_poster.jpg",
                        Categories = new() { Categories.Action, Categories.Epic},
                        Actors = new() { "Jack Black", "Jason Momoa", "Sebastian Hansen" },
                        PublicationDate = new DateTime(2025, 4, 4),
                        OriginalLanguage = "English",
                        Directors = new() { "Jared Hess" },
                        Duration = new TimeSpan(1, 41, 0),
                        AgeRating = AgeRatings.PG,
                    },
                    new()
                    {
                        Name = "Truth and Justice",
                        Description = "Andres settles in a new farm. With rose-tinted lenses, he sets out to transform the farm into a prosperous paradise - all he needs is a healthy harvest, cooperative neighbors and a son. But, life turns out to be cruel and treacherous.",
                        Rating = Ratings.VeryGood,
                        Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/88/TODE_JA_OIGUS_p%C3%B5hiposter.jpg/330px-TODE_JA_OIGUS_p%C3%B5hiposter.jpg",
                        Categories = new() { Categories.Drama },
                        Actors = new() { "Priit Loog", "Ester Kuntu", "Priit Võigemast" },
                        PublicationDate = new DateTime(2019, 2, 22),
                        OriginalLanguage = "Estonian",
                        Directors = new() { "Tanel Toom" },
                        Duration = new TimeSpan(2, 45, 0),
                        AgeRating = AgeRatings.PG_13,
                    },
                    new()
                    {
                        Name = "Winnie-the-Pooh: Blood and Honey",
                        Description = "After Christopher Robin abandons them for college, Pooh and Piglet embark on a bloody rampage as they search for a new source of food.",
                        Rating = Ratings.Negative,
                        Image = "https://upload.wikimedia.org/wikipedia/en/7/74/Winnie_the_Pooh%2C_Blood_and_Honey_Film_Poster.jpg",
                        Categories = new() { Categories.Horror, Categories.Monster },
                        Actors = new() { "Nikolai Leon", "Maria Taylor", "Natasha Rose Mills" },
                        PublicationDate = new DateTime(2023, 3, 17),
                        OriginalLanguage = "English",
                        Directors = new() { "Rhys Frake-Waterfield" },
                        Duration = new TimeSpan(1, 24, 0),
                        AgeRating = AgeRatings.Not_Rated,
                    },
                    new()
                    {
                        Name = "Cars",
                        Description = "On the way to the biggest race of his life, a hotshot rookie race car gets stranded in a rundown town and learns that winning isn't everything in life.",
                        Rating = Ratings.Good,
                        Image = "https://upload.wikimedia.org/wikipedia/en/3/34/Cars_2006.jpg",
                        Categories = new() { Categories.Comedy, Categories.Animation, Categories.Motorsport, Categories.Adventure, Categories.Family },
                        Actors = new() { "Owen Wilson", "Bonnie Hunt", "Paul Newman" },
                        PublicationDate = new DateTime(2006, 6, 9),
                        OriginalLanguage = "English",
                        Directors = new() { "John Lasseter", "Joe Ranft" },
                        Duration = new TimeSpan(1, 56, 0),
                        AgeRating = AgeRatings.G,
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
