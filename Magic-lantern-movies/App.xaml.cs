#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

using Services;
using System.Diagnostics;

namespace Magic_lantern_movies
{
    public partial class App : Application
    {
        const int WindowWidth = 540;
        const int WindowHeight = 1000;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            try
            {
                // Initialize services asynchronously
                Task.Run(async () =>
                {
                    var moviesService = MauiProgram.Services.GetRequiredService<MoviesService>();
                    await moviesService.InitializeMoviesAsync();
                }).Wait(); // Only for demonstration; prefer async startup in real apps
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }


            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
#if WINDOWS
        var mauiWindow = handler.VirtualView;
        var nativeWindow = handler.PlatformView;
        nativeWindow.Activate();
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
        WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
        AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
#endif
            });
        }
    }
}