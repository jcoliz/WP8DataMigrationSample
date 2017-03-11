using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist.Portable.Models;

namespace Todolist.Uwp.Models
{
    /// <summary>
    /// Sqlite database interaction
    /// </summary>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/ef/core/get-started/uwp/getting-started
    /// </remarks>
    public class TodoDbContext: DbContext
    {
        public DbSet<TodoItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Todo.db");
        }
    }
}
