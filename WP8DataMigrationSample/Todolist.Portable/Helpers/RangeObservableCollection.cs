using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todolist.Portable.Helpers
{
    /// <summary>
    /// This implementation improves the behaviour of the standard ObservableCollection when dealing with a large number of elements. This collection does not fire an event for each inserted element, but fires a reset event when adding elements
    /// </summary>
    /// <remarks>
    /// https://ireadable.wordpress.com/2014/10/20/rangeobservablecollection-pragmatic-implementation/
    /// </remarks>
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private static readonly NotifyCollectionChangedEventArgs ResetChangedArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        private static readonly PropertyChangedEventArgs CountChangedEventArgs = new PropertyChangedEventArgs("Count");
        private static readonly PropertyChangedEventArgs ItemChangedEventArgs = new PropertyChangedEventArgs("Item[]");

        public RangeObservableCollection(IEnumerable<T> items)
        : base(items)
        {
        }

        public RangeObservableCollection(List<T> list)
        : base(list)
        {
        }

        public RangeObservableCollection()
        {
        }

        public void RemoveRange(IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            foreach (T item in list)
            {
                Items.Remove(item);
            }

            OnCollectionChanged(ResetChangedArgs);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(ItemChangedEventArgs);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");

            foreach (T item in list)
            {
                Items.Add(item);
            }
            OnCollectionChanged(ResetChangedArgs);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(ItemChangedEventArgs);
        }

        public void RemoveAll()
        {
            Items.Clear();
            OnCollectionChanged(ResetChangedArgs);
            OnPropertyChanged(CountChangedEventArgs);
            OnPropertyChanged(ItemChangedEventArgs);
        }

        public void ReplaceAll(IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException("list");
            Items.Clear();
            this.AddRange(list);
        }
    }
}
