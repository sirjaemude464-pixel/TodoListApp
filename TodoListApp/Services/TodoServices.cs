using System;
using System.Collections.Generic;
using System.Linq;
using TodoListApp.Models;
using TodoListApp.Repositories;

namespace TodoListApp.Services
{
    public class TodoService
    {
        private readonly List<TodoModel> _tasks;
        private readonly ITaskRepository _repository;

        public TodoService(ITaskRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tasks = new List<TodoModel>();
        }

        public IReadOnlyList<TodoModel> GetAllTasks() => _tasks.AsReadOnly();

        public void AddTask(TodoModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _tasks.Add(task);
        }

        public bool UpdateTask(Guid id, string newTitle, string newDescription, DateTime newDueDate, TaskPriority newPriority)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;

            task.Title = newTitle;
            task.Description = newDescription;
            task.DueDate = newDueDate;
            task.Priority = newPriority;
            return true;
        }

        public bool DeleteTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return task != null && _tasks.Remove(task);
        }

        public bool UpdateStatus(Guid id, TaskItemStatus newStatus)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                return false;

            task.Status = newStatus;
            return true;
        }

        public void SaveToFile(string filePath)
        {
            _repository.Save(_tasks, filePath);
        }

        public void LoadFromFile(string filePath)
        {
            var loaded = _repository.Load(filePath);
            _tasks.Clear();
            _tasks.AddRange(loaded);
        }
    }
}