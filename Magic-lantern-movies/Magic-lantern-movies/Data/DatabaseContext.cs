using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DatabaseContext
    {
        private const string DbName = "MOVIEdb";
        private static string DbPath => Path.Combine(".", DbName);

        private SQLiteAsyncConnection _connection;

        private SQLiteAsyncConnection Database =>
            (_connection ??= new SQLiteAsyncConnection(DbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));

        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }
    }
}
