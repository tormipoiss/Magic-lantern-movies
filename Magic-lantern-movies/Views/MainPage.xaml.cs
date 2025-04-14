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

            SizeChanged += OnPageSizeChanged; // Attach to the page's SizeChanged event

            Log.Information("MainPage is loaded.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            Log.Error(ex, "An error occurred");
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
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
}