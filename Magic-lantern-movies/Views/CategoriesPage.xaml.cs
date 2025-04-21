using Serilog;
using Services;
using System.Diagnostics;
using ViewModels;

namespace Views;

public partial class CategoriesPage : ContentPage
{
    CategoriesViewModel _viewModel = ServiceLocator.GetService<CategoriesViewModel>();

    public CategoriesPage()
    {
        try
        {
            InitializeComponent();

            BindingContext = _viewModel;
            _viewModel.InitializeAsync();

            SizeChanged += OnPageSizeChanged; // Attach to the page's SizeChanged event

            Log.Information("CategoriesPage is loaded.");
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
}