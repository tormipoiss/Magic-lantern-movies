using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;
using Services;
using Views;

namespace ViewModels
{
    public partial class TemporaryHomePageViewModel : ObservableObject
    {
        private readonly MoviesService _moviesService;
        private readonly DatabaseContext _databaseContext;
        public TemporaryHomePageViewModel(MoviesService moviesService,DatabaseContext databaseContext)
        {
            _moviesService = moviesService;
            _databaseContext = databaseContext;
        }
        public ObservableCollection<Movie> Movies { get; set; } = new();
        private bool _init = true;

        public bool _isLoading = false;
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
        private async Task InitMovies()
        {
            await Task.Yield();

            await _moviesService.InitializeMoviesAsync();
            foreach (var movie in await _databaseContext.GetMoviesAsync())
            {
                Movies.Add(movie);
            }
            _init = false;
        }
        public async Task Init()
        {
            await Task.Yield();
            if (_init)
            {
                IsLoading = true;  // Show loading screen when task starts
                await Task.Run(async () =>
                {
                    await InitMovies(); // Ensure InitMovies runs asynchronously
                    await Task.Yield();
                });
                await Task.Yield();
                IsLoading = false;  // Hide loading screen when task finishes
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
