
using System.Diagnostics.CodeAnalysis;

namespace TodoList.Data.Models;

public class TodoItem
{
    public int Id { get; set; }
    [NotNull]
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}
