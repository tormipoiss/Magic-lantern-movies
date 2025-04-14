using ViewModels;

namespace Views;

public partial class FilmDetails : ContentPage
{
	private readonly MovieDetailsViewModel vm = new();
	public FilmDetails()
	{
		InitializeComponent();
		BindingContext = vm;
	}
}