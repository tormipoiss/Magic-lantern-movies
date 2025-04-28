using Views;

namespace Magic_lantern_movies
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MoviesByCategoryPage), typeof(MoviesByCategoryPage));
        }
    }
}