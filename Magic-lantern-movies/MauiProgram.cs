using Data;
using Microsoft.Extensions.Logging;
using Services;

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

        var app = builder.Build();
        Services = app.Services; // Expose the IServiceProvider

        return app;
    }
}