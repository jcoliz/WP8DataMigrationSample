using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todolist.Portable.Helpers;
using Todolist.Wp8.Models;

namespace Todolist.Wp8.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        public RangeObservableCollection<TodoItem> Items { get; } = new RangeObservableCollection<TodoItem>();

        public string AddMeName { get; set; } = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;
        
        // Data context for the local database
        private TodoDbContext DB;

        public void Load()
        {
            DB = new TodoDbContext(TodoDbContext.DBConnectionString);

            var items = from TodoItem item in DB.Items select item;

            Items.Clear();
            Items.AddRange(items);
        }

        public ICommand AddCommand => new DelegateCommand(_ => 
        {
            if (!string.IsNullOrEmpty(AddMeName))
            {
                var now = DateTime.Now;
                var item = new TodoItem() { Name = AddMeName, Checked = false, Created = now };
                Add(item);
                AddMeName = string.Empty;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddMeName)));
            }
        });

        public void Update(TodoItem item)
        {
            DB.SubmitChanges();
        }

        protected void Add(TodoItem item)
        {
            Items.Add(item);
            DB.Items.InsertOnSubmit(item);
            DB.SubmitChanges();
        }
    }
}
