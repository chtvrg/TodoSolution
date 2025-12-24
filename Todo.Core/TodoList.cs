using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json; 
using System.IO;        

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();
        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }

        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;

        public IEnumerable<TodoItem> Find(string substring) =>
            _items.Where(i => i.Title.Contains(substring ?? string.Empty,
           StringComparison.OrdinalIgnoreCase));

        public int Count => _items.Count;

        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var loadedItems = JsonSerializer.Deserialize<List<TodoItem>>(json);

            _items.Clear();
            if (loadedItems != null)
            {
                _items.AddRange(loadedItems);
            }
        }
    }
}