using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;
using Services;
using Views;

namespace ViewModels
{
    public partial class TemporaryHomePageViewModel
    {
        private readonly MoviesService _moviesService;
        private readonly DatabaseContext _databaseContext;
        public TemporaryHomePageViewModel(MoviesService moviesService,DatabaseContext databaseContext)
        {
            _moviesService = moviesService;
            _databaseContext = databaseContext;
        }
        public ObservableCollection<Movie> Movies { get; set; } = new();
        public async Task Init()
        {
            await _moviesService.InitializeMoviesAsync();
            foreach(var movie in await _databaseContext.GetMoviesAsync())
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
