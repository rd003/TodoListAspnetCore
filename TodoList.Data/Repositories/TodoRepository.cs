﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TodoList.Data.Models;

namespace TodoList.Data.Repositories;

public interface ITodoRepository
{
    Task<TodoItem> AddTodoItem(TodoItem item);
    Task<IEnumerable<TodoItem>> GetTodoItems();
    Task DeleteTodoItem(int id);
    Task<TodoItem?> GetTodoItemById(int id);
    Task UpdateTodoItem(TodoItem item);
}

public class TodoRepository : ITodoRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public TodoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("default") ?? "";
    }

    public async Task<TodoItem> AddTodoItem(TodoItem item)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "insert into TodoItem(Description,IsCompleted) values(@Description,@IsCompleted); select SCOPE_IDENTITY()";
        int createdId = await connection.ExecuteScalarAsync<int>(sql, new { item.Description, item.IsCompleted });
        item.Id = createdId;
        return item;
    }

    public async Task UpdateTodoItem(TodoItem item)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "update TodoItem set Description=@Description,IsCompleted=@IsCompleted where Id=@Id";
        await connection.ExecuteAsync(sql, item);
    }

    public async Task<TodoItem?> GetTodoItemById(int id)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "select * from TodoItem where Id =@id";
        TodoItem? todoItem = await connection.QueryFirstOrDefaultAsync<TodoItem>(sql, new { id });
        return todoItem;
    }

    public async Task DeleteTodoItem(int id)
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "delete from TodoItem where Id=@id";
        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task<IEnumerable<TodoItem>> GetTodoItems()
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        string sql = "select * from TodoItem";
        var todoItems = await connection.QueryAsync<TodoItem>(sql);
        return todoItems;
    }

}
