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
    [Table]
    public class TodoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    NotifyPropertyChanging(nameof(Id));
                    _Id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }
        private int _Id;

        [Column]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    NotifyPropertyChanging(nameof(Name));
                    _Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        private string _Name;

        [Column]
        public bool? Checked
        {
            get
            {
                return _Checked;
            }
            set
            {
                if (_Checked != value)
                {
                    NotifyPropertyChanging(nameof(Checked));
                    _Checked = value;
                    NotifyPropertyChanged(nameof(Checked));
                }
            }
        }
        private bool? _Checked;

        [Column]
        public DateTime Created
        {
            get
            {
                return _Created;
            }
            set
            {
                if (_Created != value)
                {
                    NotifyPropertyChanging(nameof(Created));
                    _Created = value;
                    NotifyPropertyChanged(nameof(Created));
                }
            }
        }
        private DateTime _Created;

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
