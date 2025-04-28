using CommunityToolkit.Mvvm.ComponentModel;
using Data;
using Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
            await LoadMoviesAsync();
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

        private async Task LoadMoviesAsync()
        {
            var allMovies = await _db.GetMoviesAsync();
            if (allMovies == null)
            {
                Debug.WriteLine("No movies found.");
                Text = "No movies found!";
                return;
            }

            if (Enum.TryParse<Categories>(category, out var categoryEnum))
            {
                var categoryMovies = allMovies.Where(m => m.Categories.Contains(categoryEnum)).ToList();

                if (categoryMovies == null || categoryMovies.Count == 0)
                {
                    Text = $"No movies in the category '{category}' were found!";
                }
                else
                {
                    categoryMovies = categoryMovies.OrderBy(m => m.NumberedRating).ToList();

                    foreach (var movie in categoryMovies)
                    {
                        Movies.Add(movie);
                    }

                    Text = $"Displaying movies from category: '{category}', ordered by rating score";
                }
            }
            else
            {
                Text = $"Category could not be parsed to enum: '{category}'";
                return;
            }
        }
    }
}
