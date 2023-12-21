using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace NativeAOT;

public class Database(DbConnection _connection)
{
    public async Task<Todo?> GetById(int id)
    {
        await _connection.OpenAsync();

        var command = _connection.CreateCommand();
        command.CommandText = "select Id, Title, Description from Todo Where Id = @Id";
        command.Parameters.Add(new SqliteParameter("@Id", id));

        using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        await reader.ReadAsync();

        var result = new Todo(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2)
        );

        await _connection.CloseAsync();

        return result;
    }

    public async Task<List<Todo>> GetAll()
    {
        await _connection.OpenAsync();

        var command = _connection.CreateCommand();
        command.CommandText = "select Id, Title, Description from Todo";
        using var reader = await command.ExecuteReaderAsync();
        List<Todo> results = new();
        while (await reader.ReadAsync())
        {
            results.Add(
                new Todo(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2)
                )
            );
        }

        await _connection.CloseAsync();

        return results;
    }

    public async void Insert(Todo todo)
    {
        await _connection.OpenAsync();

        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO Todo VALUES(@Id, @Title, @Description)";
        command.Parameters.Add(new SqliteParameter("@Id", todo.Id));
        command.Parameters.Add(new SqliteParameter("@Title", todo.Title));
        command.Parameters.Add(new SqliteParameter("@Description", todo.Description));

        await command.ExecuteNonQueryAsync();

        await _connection.CloseAsync();
    }
}
