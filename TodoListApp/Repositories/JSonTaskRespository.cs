using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TodoListApp.Models;

namespace TodoListApp.Repositories
{
    public class JsonTaskRepository : ITaskRepository
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonTaskRepository()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        }

        public void Save(IEnumerable<TodoModel> tasks, string filePath)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            string json = JsonSerializer.Serialize(tasks, _jsonOptions);
            File.WriteAllText(filePath, json);
        }

        public List<TodoModel> Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<TodoModel>();

            string json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<TodoModel>();

            return JsonSerializer.Deserialize<List<TodoModel>>(json, _jsonOptions) ?? new List<TodoModel>();
        }
    }
}