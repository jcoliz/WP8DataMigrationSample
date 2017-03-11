using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Portable.Models
{
    /// <summary>
    /// A single todo item
    /// </summary>
    public class TodoItem
    {
        public int Id { get; set; }
        public bool? Checked { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
