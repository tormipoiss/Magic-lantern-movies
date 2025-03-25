using Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DatabaseContext
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseContext(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Movie>().Wait();
        }

        public Task<List<Movie>> GetMoviesAsync()
        {
            return _database.Table<Movie>().ToListAsync();
        }

        public Task<int> SaveMovieAsync(Movie movie)
        {
            return _database.InsertAsync(movie);
        }
    }
}
