using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Views;

namespace ViewModels
{
    [QueryProperty(nameof(Category), "Category")]
    public partial class MoviesByCategoryViewModel : ObservableObject
    {
        public ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();

        private readonly DatabaseContext _db;

        [ObservableProperty]
        string category;

        [ObservableProperty]
        private int columnSpan = 1; // Default to 1 column

        [ObservableProperty]
        private string text = $"";

        private List<Movie> categoryMovies;

        public MoviesByCategoryViewModel(DatabaseContext db)
        {
            try
            {
                _db = db;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Log.Error(ex, "An error occurred");
            }
        }

        public async Task InitializeAsync()
        {
            await LoadMoviesAsync("Ranking top");
            Debug.WriteLine("MoviesByCategoryViewModel initialize method activated");
            Log.Information("MoviesByCategoryViewModel initialize method activated");
        }

        public void UpdateSpan(double width)
        {
            Debug.WriteLine($"Screen width: {width}");

            if (width >= 768) // Example breakpoint for larger screens
            {
                ColumnSpan = 2; // Use 2 columns for larger screens
            }
            else
            {
                ColumnSpan = 1; // Use 1 column for smaller screens
            }
        }

        public async Task LoadMoviesAsync(string method)
        {
            if (categoryMovies == null || categoryMovies.Count == 0)
            {
                var allMovies = await _db.GetMoviesAsync();
                if (allMovies == null)
                {
                    Debug.WriteLine("No movies found.");
                    Text = "No movies found!";
                    return;
                }
                else
                {
                    categoryMovies = allMovies.Where(m => m.Categories.Contains(Category)).ToList();

                    if (categoryMovies == null || categoryMovies.Count == 0)
                    {
                        Text = $"No movies in the category '{Category}' were found!";
                        return;
                    }
                }
            }
            Text = $"Displaying movies from category: {Category}";

            Movies.Clear();

            switch (method)
            {
                case "Ranking top":
                    await GetBestMoviesAsync();
                    return;
                case "Ranking bottom":
                    await GetWorstMoviesAsync();
                    return;
                case "Release date bottom":
                    await GetOldestMoviesAsync();
                    return;
                case "Release date top":
                    await GetNewestMoviesAsync();
                    return;
                case "Runtime bottom":
                    await GetShortestMoviesAsync();
                    return;
                case "Runtime top":
                    await GetLongestMoviesAsync();
                    return;
                case "Alphabetical top":
                    await GetAtoYMoviesAsync();
                    return;
                case "Alphabetical bottom":
                    await GetYtoAMoviesAsync();
                    return;
                default:
                    Debug.WriteLine($"The movie getting method: {method} does not exist.");
                    return;
            }
        }

        private async Task GetYtoAMoviesAsync()
        {
            var YtoAMovies = categoryMovies.OrderByDescending(m => m.Name).ToList();

            foreach (var movie in YtoAMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetAtoYMoviesAsync()
        {
            var AtoYMovies = categoryMovies.OrderBy(m => m.Name).ToList();

            foreach (var movie in AtoYMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetLongestMoviesAsync()
        {
            var longestMovies = categoryMovies.OrderByDescending(m => m.Duration).ToList();

            foreach (var movie in longestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetShortestMoviesAsync()
        {
            var shortestMovies = categoryMovies.OrderBy(m => m.Duration).ToList();

            foreach (var movie in shortestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetNewestMoviesAsync()
        {
            var newestMovies = categoryMovies.OrderByDescending(m => m.PublicationDateTicks).ToList();

            foreach (var movie in newestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetOldestMoviesAsync()
        {
            var oldestMovies = categoryMovies.OrderBy(m => m.PublicationDateTicks).ToList();

            foreach (var movie in oldestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetBestMoviesAsync()
        {
            var bestMovies = categoryMovies.OrderBy(m => m.NumberedRating).ToList();

            foreach (var movie in bestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetWorstMoviesAsync()
        {
            var worstMovies = categoryMovies.OrderByDescending(m => m.NumberedRating).ToList();

            foreach (var movie in worstMovies)
            {
                Movies.Add(movie);
            }
        }
        [RelayCommand]
        private async Task GoTo(Movie movie)
        {
            var param = new Dictionary<string, object>()
            {
                [nameof(MovieDetailsViewModel.CurrentMovie)] = movie
            };
            await Shell.Current.GoToAsync(nameof(FilmDetails), animate: true, param);
        }
    }
}
