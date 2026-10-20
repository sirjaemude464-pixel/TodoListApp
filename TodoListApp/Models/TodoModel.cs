using System;

namespace TodoListApp.Models
{
    public class TodoModel
    {
        private Guid _id;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _createdAt;
        private DateTime _dueDate;
        private TaskPriority _priority;
        private TaskItemStatus _status;

        public TodoModel()
        {
            _id = Guid.NewGuid();
            _createdAt = DateTime.Now;
            _dueDate = DateTime.Now.AddDays(1);
            _priority = TaskPriority.Medium;
            _status = TaskItemStatus.NotStarted;
        }

        public TodoModel(string title, string description, DateTime dueDate, TaskPriority priority, TaskItemStatus status = TaskItemStatus.NotStarted)
        {
            _id = Guid.NewGuid();
            _createdAt = DateTime.Now;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = status;
        }

        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

        // Инкапсуляция с проверкой на пустое название
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название задачи не может быть пустым.");
                _title = value.Trim();
            }
        }

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set => _dueDate = value;
        }

        public TaskPriority Priority
        {
            get => _priority;
            set => _priority = value;
        }

        public TaskItemStatus Status
        {
            get => _status;
            set => _status = value;
        }
    }
}