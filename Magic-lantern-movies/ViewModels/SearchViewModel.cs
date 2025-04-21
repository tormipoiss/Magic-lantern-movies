using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;
using Views;

namespace ViewModels
{
    partial class SearchViewModel : ObservableObject
    {
        private readonly DatabaseContext _databaseContext;

        public SearchViewModel(DatabaseContext databaseContext)
        {
            SearchCommand = new AsyncRelayCommand(OnSearchClicked);
            _databaseContext = databaseContext;
            FoundMovies = new ObservableCollection<Movie>();
            Task.Run(() => SetSearchPlaceholder());
        }
        private async Task SetSearchPlaceholder()
        {
            int totalMovies = await _databaseContext.GetTotalMoviesAsync();
            CurrentMovies = $"Search over {totalMovies} movies!";
        }
        public ObservableCollection<Movie> FoundMovies { get; set; }

        private bool _anyMovies;
        public bool AnyMovies
        {
            get => _anyMovies;
            set => SetProperty(ref _anyMovies, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        [ObservableProperty]
        private string _currentMovies;

        public IAsyncRelayCommand SearchCommand { get; }


        private async Task OnSearchClicked()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return;
            }

            var movies = await _databaseContext.SearchMovieAsync(SearchText);
            int totalMovies = await _databaseContext.GetTotalMoviesAsync();
            FoundMovies.Clear();
            foreach (var movie in movies)
            {
                FoundMovies.Add(movie);
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
