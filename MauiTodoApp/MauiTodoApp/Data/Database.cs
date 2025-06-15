using MauiTodoApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTodoApp.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _connection;

        public Database()
        {
            string dataDir = FileSystem.AppDataDirectory;

            var databasePath = Path.Combine(dataDir, "SecuredMauiTodoApp.db");

            string? key = SecureStorage.GetAsync("dbkey").Result;

            if (string.IsNullOrEmpty(key))
            {
                key = new Guid().ToString();
                SecureStorage.SetAsync("dbkey", key); //store my key securely
            }

            // prepare the conection string
            var dbOptions = new SQLiteConnectionString(databasePath, true, key);

            _connection = new SQLiteAsyncConnection(dbOptions);

            _ = InitializeTables();
        }

        private async Task InitializeTables()
        {
            await _connection.CreateTableAsync<TodoItem>();
        }

        public async Task<List<TodoItem>> GetTodos()
        {
            return await _connection.Table<TodoItem>().ToListAsync();
        }

        public async Task<TodoItem> GetTodo(int id)
        {
            var query = _connection
                .Table<TodoItem>()
                .Where(item => item.ID == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> AddTodo(TodoItem item)
        {
            return await _connection.InsertAsync(item);
        }

        public async Task<int> UpdateTodo(TodoItem item)
        {
            return await _connection.UpdateAsync(item);
        }

        public async Task<int> DeleteTodo(int id)
        {
            var item = GetTodo(id);
            return await _connection.DeleteAsync(item);
        }

        public async Task<int> DeleteTodo(TodoItem item)
        {
            return await _connection.DeleteAsync(item);
        }
    }
}
