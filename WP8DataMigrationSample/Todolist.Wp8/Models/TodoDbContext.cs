using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Wp8.Models
{
    /// <summary>
    /// Database context
    /// </summary>
    /// <remarks>
    /// https://msdn.microsoft.com/en-us/library/windows/apps/hh202876(v=vs.105).aspx
    /// https://msdn.microsoft.com/en-us/library/windows/apps/hh202860(v=vs.105).aspx
    /// </remarks>
    public class TodoDbContext: DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/Todo.sdf";

        // Pass the connection string to the base class.
        public TodoDbContext(string connectionString): base(connectionString) { }

        // Specify a single table for the to-do items.
        public Table<TodoItem> Items;
    }

}
