using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Todolist.Wp8.Resources;
using Todolist.Wp8.Models;
using Todolist.Wp8.ViewModels;

namespace Todolist.Wp8
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainViewModel VM { get; } = new MainViewModel();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            VM.Load();

            base.OnNavigatedTo(e);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var item = checkbox.DataContext as TodoItem;
            if (null != item)
            {
                // This event fires BEFORE the change gets updated in the model
                item.Checked = checkbox.IsChecked;
                // Persist the change to DB
                VM.Update(item);
            }
        }
    }
}