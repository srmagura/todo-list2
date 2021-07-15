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

            var cmdText = "insert into Todos values (@id, @done, @label)";
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@id", todo.Id);
            cmd.Parameters.AddWithValue("@done", 0);
            cmd.Parameters.AddWithValue("@label", todo.Label);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task SetDoneAsync(Guid id, bool done)
        {
            using var conn = await GetConnectionAsync();

            var cmdText = "update Todos set Done=@done where Id=@id";
            using var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@done", done);
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Todo>> ListAsync()
        {
            using var conn = await GetConnectionAsync();

            var cmdText = "select * from Todos";
            using var cmd = new SqlCommand(cmdText, conn);

            using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<Todo>();

            while (await reader.ReadAsync())
            {
                var todo = new Todo(
                    (bool)reader["Done"],
                    (string)reader["Label"]
                )
                {
                    Id = (Guid)reader["Id"]
                };

                result.Add(todo);
            }

            return result;
        }
    }
}
