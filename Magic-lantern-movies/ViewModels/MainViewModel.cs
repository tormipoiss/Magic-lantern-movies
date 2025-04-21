using CommunityToolkit.Mvvm.ComponentModel;
using Data;
using Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Movie> Movies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> OtherMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> BestMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> RecentMovies { get; set; } = new ObservableCollection<Movie>();

        private readonly DatabaseContext _db;

        [ObservableProperty]
        private int columnSpan = 1; // Default to 1 column

        private List<Movie> allMovies;

        public MainViewModel(DatabaseContext db)
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
            await LoadMoviesAsync("Default movies");
            Debug.WriteLine("MainViewModel initialize method activated");
            Log.Information("MainViewModel initialize method activated");
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
            if (allMovies == null || allMovies.Count == 0)
            {
                allMovies = await _db.GetMoviesAsync();
                if (allMovies == null)
                {
                    Debug.WriteLine("No movies found.");
                    return;
                }
            }

            Movies.Clear();

            switch (method)
            {
                case "Default movies":
                    await GetDefaultMoviesAsync();
                    return;
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

        private async Task GetDefaultMoviesAsync()
        {
            var bestMovies = allMovies.Where(m => m.Rating == Ratings.VeryGood).ToList();
            var recentMovies = allMovies.OrderByDescending(m => m.PublicationDateTicks).Take(4).ToList();

            foreach (var movie in bestMovies)
            {
                BestMovies.Add(movie);
            }

            foreach (var movie in recentMovies)
            {
                RecentMovies.Add(movie);
            }

            var otherMovies = allMovies.Except(bestMovies.Concat(recentMovies)).ToList();

            foreach (var movie in otherMovies)
            {
                OtherMovies.Add(movie);
            }

            var bestToLowMovies = allMovies.OrderBy(m => m.NumberedRating).ToList();

            foreach (var movie in bestToLowMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetYtoAMoviesAsync()
        {
            var YtoAMovies = allMovies.OrderByDescending(m => m.Name).ToList();

            foreach (var movie in YtoAMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetAtoYMoviesAsync()
        {
            var AtoYMovies = allMovies.OrderBy(m => m.Name).ToList();

            foreach (var movie in AtoYMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetLongestMoviesAsync()
        {
            var longestMovies = allMovies.OrderByDescending(m => m.Duration).ToList();

            foreach (var movie in longestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetShortestMoviesAsync()
        {
            var shortestMovies = allMovies.OrderBy(m => m.Duration).ToList();

            foreach (var movie in shortestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetNewestMoviesAsync()
        {
            var newestMovies = allMovies.OrderByDescending(m => m.PublicationDateTicks).ToList();

            foreach (var movie in newestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetOldestMoviesAsync()
        {
            var oldestMovies = allMovies.OrderBy(m => m.PublicationDateTicks).ToList();

            foreach (var movie in oldestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetBestMoviesAsync()
        {
            var bestMovies = allMovies.OrderBy(m => m.NumberedRating).ToList();

            foreach (var movie in bestMovies)
            {
                Movies.Add(movie);
            }
        }

        private async Task GetWorstMoviesAsync()
        {
            var worstMovies = allMovies.OrderByDescending(m => m.NumberedRating).ToList();

            foreach (var movie in worstMovies)
            {
                Movies.Add(movie);
            }
        }
    }
}
