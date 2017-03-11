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
    /// <remarks>
    /// This model is used by all platforms in this project to represent a todo item
    /// </remarks>
    public class TodoItem
    {
        public int Id { get; set; }
        public bool? Checked { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        /// <summary>
        /// Migrate from a WP8 SQLCE database
        /// </summary>
        /// <param name="dictionary">Sql CE table entry in dictionary form</param>
        /// <returns>RecordedTrack with those values</returns>
        public static TodoItem MigrateFromDictionary(IReadOnlyDictionary<string, object> dictionary)
        {
            var item = new TodoItem();

            item.Id = (int)dictionary["Id"];

            if (dictionary.ContainsKey("Checked"))
                item.Checked = (bool)dictionary["TotalClimbing"];

            var name = (string)dictionary["Name"];
            if (!string.IsNullOrEmpty(name))
                item.Name = name;

            item.Created = (DateTime)dictionary["Created"];

            return item;
        }
    }

    public class TodoItemList: List<TodoItem>
    {
    }
}
