using Data;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Services;
using System.Diagnostics;
using ViewModels;
using Serilog;
using Views;

namespace Magic_lantern_movies;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Define the log file path in the project folder
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "log-.txt");

        // Ensure the "logs" directory exists
        Debug.WriteLine($"Log directory: {logFilePath}");
        if (!Directory.Exists(logFilePath))
        {
            Directory.CreateDirectory(logFilePath);
            Debug.WriteLine("Logs directory created.");
        }
        else
        {
            Debug.WriteLine("Logs directory already exists.");
        }

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                path: logFilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7, // Keep logs for the last 7 days
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
            )
            .CreateLogger();

        builder.Logging.AddSerilog();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        // Define the database path
        var dbPath = Path.Combine(".", "movies.db58");

        Debug.WriteLine($"dbPath: {dbPath}");

        // Register DatabaseContext with the dbPath parameter
        builder.Services.AddSingleton<DatabaseContext>(s => new DatabaseContext(dbPath));

        // Register MoviesService
        builder.Services.AddSingleton<MoviesService>();
        builder.Services.AddSingleton<SearchViewModel>(s =>
        {
            var db = s.GetService<DatabaseContext>();
            return new SearchViewModel(db);
        });
        try
        {
            builder.Services.AddSingleton<MainViewModel>(s =>
            {
                var db = s.GetService<DatabaseContext>();
                var mv = s.GetService<MoviesService>();
                return new MainViewModel(db,mv);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            Log.Error(ex, "An error occurred");
        }

        try
        {
            builder.Services.AddSingleton<CategoriesViewModel>(s =>
            {
                var db = s.GetService<DatabaseContext>();
                return new CategoriesViewModel(db);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            Log.Error(ex, "An error occurred");
        }

        try
        {
            builder.Services.AddTransient<MoviesByCategoryViewModel>(s =>
            {
                var db = s.GetService<DatabaseContext>();
                return new MoviesByCategoryViewModel(db);
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            Log.Error(ex, "An error occurred");
        }

        var app = builder.Build();
        ServiceLocator.ServiceProvider = app.Services;
        /*Services = app.Services;*/ // Expose the IServiceProvider

        return app;
    }
}