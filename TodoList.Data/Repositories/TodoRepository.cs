using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TodoList.Data.Models;

namespace TodoList.Data.Repositories;

public class TodoRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public TodoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("default");
    }

   
    public async Task<TodoItem> AddTodoItem(TodoItem item)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "insert into TodoItem(Description,IsCompleted) values(@Description,@IsCompleted); select SCOPE_IDENTITY()";
        int createdId = await connection.ExecuteScalarAsync<int>(sql, new { item.Description,item.IsCompleted});
        item.Id = createdId;
        return item;
    }
    public async Task UpdateTodoItem(TodoItem item)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "update TodoItem set Description=@Description,IsCompleted=@IsCompleted where Id=@Id";
        await connection.ExecuteAsync(sql, item);
    }
}
