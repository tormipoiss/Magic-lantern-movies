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

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "movies.db3");

        builder.Services.AddSingleton<DatabaseContext>(s => new DatabaseContext(dbPath));

        builder.Services.AddSingleton<MoviesService>();

        var app = builder.Build();
        Services = app.Services;

        return app;
    }
}