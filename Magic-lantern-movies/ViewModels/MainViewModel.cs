using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;
using Serilog;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Views;

namespace ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Movie> OtherMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> BestMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> RecentMovies { get; set; } = new ObservableCollection<Movie>();

        private readonly MoviesService _moviesService;

        private readonly DatabaseContext _db;

        [ObservableProperty]
        private int columnSpan = 1; // Default to 1 column

        public MainViewModel(DatabaseContext db, MoviesService moviesService)
        {
            try
            {
                _db = db;
                _moviesService = moviesService;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Log.Error(ex, "An error occurred");
            }
        }
        private bool _init = true;
        public bool _isLoading = true;
        public bool _isNotLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading)); // Notify UI about change in IsLoading
                }
            }
        }
        public bool IsNotLoading
        {
            get => _isNotLoading;
            set
            {
                if (_isNotLoading != value)
                {
                    _isNotLoading = value;
                    OnPropertyChanged(nameof(IsNotLoading)); // Notify UI about change in IsLoading
                }
            }
        }
        public async Task InitializeAsync()
        {
            if (_init)
            {
                await _moviesService.InitializeMoviesAsync();
                await LoadMoviesAsync();
                _init = false;
            }
            IsLoading = false;
            IsNotLoading = true;
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

            var bestMovies = allMovies.Where(m => m.Rating == Ratings.VeryGood || m.Rating == Ratings.Good).ToList();
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
