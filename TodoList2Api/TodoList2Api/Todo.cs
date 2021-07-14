using System;
using System.Collections.Generic;
using System.Text;

namespace TodoList2Api
{
    public class Todo
    {
        public Todo(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new Exception("Label is required.");

            Label = label;
        }

        public bool Done { get; set; }
        public string Label { get; set; }
    }
}
