//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;
//using CommunityToolkit.Mvvm.ComponentModel;
//using Data;
//using Models;

//namespace ViewModels
//{
//    public partial class MainPageViewModel : ObservableObject
//    {
//        private readonly DatabaseContext _context;
//        public MainPageViewModel(DatabaseContext context)
//        {
//            _context = context;
//        }

//        [ObservableProperty]
//        private ObservableCollection<Movie> _movies = new();

//        [ObservableProperty]
//        private bool _isBusy;

//        [ObservableProperty]
//        private string _busyText;

//        public async Task LoadFilmsAsync()
//        {
//            await ExecuteAsync(async () =>
//            {
//                var movies = await _context.GetAllAsync<Movie>();
//                if (movies is not null && movies.Any())
//                {
//                    Movies ??= new ObservableCollection<Movie>();

//                    foreach (var product in movies)
//                    {
//                        Movies.Add(product);
//                    }
//                }
//            }, "Fetching products...");
//        }

//        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
//        {
//            IsBusy = true;
//            BusyText = busyText ?? "Processing...";
//            try
//            {
//                await operation?.Invoke();
//            }
//            catch (Exception)
//            {

//                throw;
//            }
//            finally
//            {
//                IsBusy = false;
//                BusyText = "Processing...";
//            }
//        }
//    }
//}
