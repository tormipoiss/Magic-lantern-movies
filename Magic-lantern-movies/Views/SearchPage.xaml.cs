using System.Diagnostics;
using Data;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Services;
using ViewModels;

namespace Views;

public partial class SearchPage : ContentPage
{
    SearchViewModel _viewModel = ServiceLocator.GetService<SearchViewModel>();
    public SearchPage()
	{
		InitializeComponent();
		BindingContext = _viewModel;
    }
}