using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace Todolist.Wp8.Models
{
    /// <summary>
    /// Presents a Todolist.Portable.Model to the WP8 data system.
    /// </summary>
    /// <remarks>
    /// We cannot use a portable model directly, because the public properties require markup
    /// </remarks>
    [Table]
    public class TodoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private Todolist.Portable.Models.TodoItem _Item = new Todolist.Portable.Models.TodoItem();

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get
            {
                return _Item.Id;
            }
            set
            {
                if (_Item.Id != value)
                {
                    NotifyPropertyChanging(nameof(Id));
                    _Item.Id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }

        [Column]
        public string Name
        {
            get
            {
                return _Item.Name;
            }
            set
            {
                if (_Item.Name != value)
                {
                    NotifyPropertyChanging(nameof(Name));
                    _Item.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        [Column]
        public bool? Checked
        {
            get
            {
                return _Item.Checked;
            }
            set
            {
                if (_Item.Checked != value)
                {
                    NotifyPropertyChanging(nameof(Checked));
                    _Item.Checked = value;
                    NotifyPropertyChanged(nameof(Checked));
                }
            }
        }

        [Column]
        public DateTime Created
        {
            get
            {
                return _Item.Created;
            }
            set
            {
                if (_Item.Created != value)
                {
                    NotifyPropertyChanging(nameof(Created));
                    _Item.Created = value;
                    NotifyPropertyChanged(nameof(Created));
                }
            }
        }

#pragma warning disable 169
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;
#pragma warning restore

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

}
