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

namespace ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Movie> OtherMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> BestMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> RecentMovies { get; set; } = new ObservableCollection<Movie>();

        private readonly DatabaseContext _db;

        [ObservableProperty]
        private int columnSpan = 1; // Default to 1 column

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
            await LoadMoviesAsync();
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

        private async Task LoadMoviesAsync()
        {
            var allMovies = await _db.GetMoviesAsync();
            if (allMovies == null)
            {
                Debug.WriteLine("No movies found.");
                return;
            }

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
        }
    }
}
