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
            _database.CreateTableAsync<Comment>().Wait();
        }

        public Task<int> SaveMovieAsync(Movie movie)
        {
            return _database.InsertAsync(movie);
        }
        public Task<List<Movie>> GetMoviesAsync()
        {
            return _database.Table<Movie>().ToListAsync();
        }

        public Task<int> GetTotalMoviesAsync()
        {
            return _database.Table<Movie>().CountAsync();
        }
        public Task<List<Movie>> SearchMovieAsync(string searchString)
        {
            var query = "SELECT * FROM Movie WHERE Name LIKE ?";
            var movies = _database.QueryAsync<Movie>(query, "%" + searchString + "%");

            return movies;
        }
        // get by name method maybe
        //public Task<List<Movie>> GetMoviesByNameAsync(string name)
        //{
        //    return _database.Table<Movie>().Where(m => m.Name == name).ToListAsync();
        //}
    }
}
