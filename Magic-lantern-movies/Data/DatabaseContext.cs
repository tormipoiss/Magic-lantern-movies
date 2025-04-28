using Models;
using SQLite;
using True_Comment;

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

        public Task<int> SaveCommentAsync(Comment comment)
        {
            return _database.InsertAsync(comment);
        }

        public Task<List<Movie>> GetMoviesAsync()
        {
            return _database.Table<Movie>().ToListAsync();
        }

        public Task<List<Comment>> GetCommentsForMovieAsync(Guid movieId)
        {
            return _database.Table<Comment>().Where(c => c.MovieId == movieId).ToListAsync();
        }
    }
}
