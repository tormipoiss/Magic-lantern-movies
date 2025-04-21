using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data;
using Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Views;

namespace ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>();

        private readonly DatabaseContext _db;

        [ObservableProperty]
        private int columnSpan = 1; // Default to 1 column

        public CategoriesViewModel(DatabaseContext db)
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
            await LoadCategoriesAsync();
            Debug.WriteLine("CategoriesViewModel initialize method activated");
            Log.Information("CategoriesViewModel initialize method activated");
        }

        private async Task LoadCategoriesAsync()
        {
            var movies = await _db.GetMoviesAsync();
            List<string> categoryNames = new();
            foreach (var movie in movies)
            {
                foreach (var category in movie.Categories)
                {
                    if(!categoryNames.Contains(category)) categoryNames.Add(category);
                }
            }

            foreach (var category in categoryNames)
            {
                Categories.Add(category);
            }
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

        [RelayCommand]
        private async Task GoTo(string category)
        {
            var param = new Dictionary<string, object>()
            {
                { "Category", category }
            };

            Debug.WriteLine($"Category paramater when entering MoviesByCategoryPage: {param}");

            await Shell.Current.GoToAsync(nameof(MoviesByCategoryPage), animate: true, param);
        }
    }
}
