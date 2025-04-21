using Data;
using Serilog;
using Services;
using System.Diagnostics;
using ViewModels;

namespace Views;

public partial class MainPage : ContentPage
{
    MainViewModel _viewModel = ServiceLocator.GetService<MainViewModel>();

    public MainPage()
    {
        try
        {
            InitializeComponent();

            BindingContext = _viewModel;
            _viewModel.InitializeAsync();

            SizeChanged += OnPageSizeChanged; // Attach to the page's SizeChanged event

            Log.Information("MainPage is loaded.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            Log.Error(ex, "An error occurred");
        }
    }

    private void OnPageSizeChanged(object sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            var width = Width; // Use the page's Width property
            Debug.WriteLine($"Arrived at page size change with width: {width}");
            _viewModel.UpdateSpan(width);
        }
    }
    private async void OnRankingLabelTapped(object sender, EventArgs e)
    {
        string[] sortOptions = { "Ranking", "Release date", "Alphabetical", "Runtime" };
        string selected = await DisplayActionSheet(
            "Sort by", // Title
            "Cancel",   // Cancel button text
            null,       // Destructive button text (optional)
            sortOptions // The options
        );

        if (selected != null && selected != "Cancel")
        {
            SortingLabel.Text = selected;

            var fileSource = SortingOrderImage.Source as FileImageSource;
            if (fileSource.File == "arrow_down.png")
            {
                await _viewModel.LoadMoviesAsync($"{selected} bottom");
            }
            else
            {
                await _viewModel.LoadMoviesAsync($"{selected} top");
            }
        }
    }

    private async void OnSortingOrderTapped(object sender, EventArgs e)
    {
        var fileSource = SortingOrderImage.Source as FileImageSource;
        if (fileSource.File == "arrow_down.png")
        {
            SortingOrderImage.Source = "arrow_up.png";

            await _viewModel.LoadMoviesAsync($"{SortingLabel.Text} top");
        }
        else
        {
            SortingOrderImage.Source = "arrow_down.png";

            await _viewModel.LoadMoviesAsync($"{SortingLabel.Text} bottom");
        }
    }

    private void OnEnableSortingClicked(object sender, EventArgs e)
    {
        SortingStack.IsVisible = true;
        DefaultStack.IsVisible = false;
    }

    private void OnDisableSortingClicked(object sender, EventArgs e)
    {
        SortingStack.IsVisible = false;
        DefaultStack.IsVisible = true;
    }
}