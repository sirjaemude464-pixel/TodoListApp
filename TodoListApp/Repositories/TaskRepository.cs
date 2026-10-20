using System.Collections.Generic;
using TodoListApp.Models;

namespace TodoListApp.Repositories
{
    public interface ITaskRepository
    {
        void Save(IEnumerable<TodoModel> tasks, string filePath);
        List<TodoModel> Load(string filePath);
    }
}