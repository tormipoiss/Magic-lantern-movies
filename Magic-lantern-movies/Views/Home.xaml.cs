using ViewModels;

namespace Views;

public partial class Home : ContentPage
{
    private readonly TemporaryHomePageViewModel _vm;
	public Home(TemporaryHomePageViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.Init();
    }
}