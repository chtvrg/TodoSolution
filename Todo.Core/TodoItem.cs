using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Todo.Core
{
    public class TodoItem
    {
        public Guid Id { get; init; } = Guid.NewGuid(); 
        public string Title { get; init; }
        [JsonInclude]
        public bool IsDone { get; private set; }

        public TodoItem(string title)
        {
            Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
        }

        public void MarkDone() => IsDone = true;
        public void MarkUndone() => IsDone = false;      
    }
}