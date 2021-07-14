using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace TodoList2Api
{
    public class SqlTodoRepository : ITodoRepository
    {
        private async Task<SqlConnection> GetConnectionAsync()
        {
            var connectionString = Environment.GetEnvironmentVariable("Database", EnvironmentVariableTarget.Process);
            var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            return conn;
        }

        public async Task AddAsync(Todo todo)
        {
            using var conn = await GetConnectionAsync();

            var cmdText = "insert into Todos values (@done, @label)";
            using var cmd = new SqlCommand(cmdText, conn);

            await cmd.ExecuteNonQueryAsync();
        }

        public Task<List<Todo>> ListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
