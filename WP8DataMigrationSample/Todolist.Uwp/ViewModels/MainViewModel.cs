using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Todolist.Uwp.Helpers;
using Todolist.Uwp.Models;

namespace Todolist.Uwp.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        public RangeObservableCollection<TodoItem> Items { get; } = new RangeObservableCollection<TodoItem>();

        public string AddMeName { get; set; } = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load()
        {
            using (var db = new TodoDbContext())
            {
                Items.Clear();
                Items.AddRange(db.Items);
            }
        }

        public ICommand AddCommand => new DelegateCommand(_ => 
        {
            if (!string.IsNullOrEmpty(AddMeName))
            {
                var now = DateTime.Now;
                var item = new TodoItem() { Name = AddMeName, Checked = false, Created = now, Modified = now };
                Add(item);
                AddMeName = string.Empty;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddMeName)));
            }
        });

        public void Update(TodoItem item)
        {
            using (var db = new TodoDbContext())
            {
                db.Items.Update(item);
                db.SaveChanges();
            }
        }

        protected void Add(TodoItem item)
        {
            Items.Add(item);

            using (var db = new TodoDbContext())
            {
                db.Items.Add(item);
                db.SaveChanges();
            }
        }
    }
}
