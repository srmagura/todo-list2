using System;
using System.Collections.Generic;
using System.Text;

namespace TodoList2Api
{
    public class Todo
    {
        public Todo(bool done, string label)
        {
            Done = done;

            if (string.IsNullOrWhiteSpace(label))
                throw new Exception("Label is required.");

            Label = label;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public bool Done { get; set; }
        public string Label { get; set; }
    }
}
