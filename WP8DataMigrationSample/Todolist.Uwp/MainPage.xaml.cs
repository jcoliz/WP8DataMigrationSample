using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Todolist.Portable.Models;
using Todolist.Uwp.Models;
using Todolist.Uwp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Todolist.Uwp
{
    /// <summary>
    /// Main view of todo items with ability to add a new one
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel VM = new MainViewModel();

        public MainPage()
        {
            this.InitializeComponent();
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
