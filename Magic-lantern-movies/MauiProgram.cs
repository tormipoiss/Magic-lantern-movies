using Data;
using Microsoft.Extensions.Logging;
using Services;
using ViewModels;
using Views;

namespace Magic_lantern_movies;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        // Define the database path
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "movies.db3");

        // Register DatabaseContext with the dbPath parameter
        builder.Services.AddSingleton<DatabaseContext>(s => new DatabaseContext(dbPath));

        // Register MoviesService
        builder.Services.AddSingleton<MoviesService>();
        builder.Services.AddSingleton<TemporaryHomePageViewModel>();
        builder.Services.AddSingleton<Home>();
        //builder.Services.AddSingleton<MovieDetailsViewModel>();

        var app = builder.Build();
        Services = app.Services; // Expose the IServiceProvider
        //builder.Services.AddSingleton<DatabaseContext>();
        //builder.Services.AddSingleton<MoviesService>();

        return app;
    }
}