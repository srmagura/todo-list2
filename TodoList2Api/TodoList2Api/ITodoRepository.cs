using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodoList2Api
{
    public interface ITodoRepository
    {
        Task AddAsync(Todo todo);
        Task<List<Todo>> ListAsync();
    }
}
