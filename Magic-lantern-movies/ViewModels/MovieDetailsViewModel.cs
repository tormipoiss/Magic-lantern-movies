using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;

namespace ViewModels
{
    [QueryProperty(nameof(CurrentMovie), nameof(CurrentMovie))]
    public partial class MovieDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private Movie _currentMovie;
        public MovieDetailsViewModel()
        {
            
        }
    }
}
