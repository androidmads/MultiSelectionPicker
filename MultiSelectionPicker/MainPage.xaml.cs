using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MultiSelectionPicker
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            picker.ItemsSource = new List<string>() { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5", "Item 6" };
            picker.SelectedIndices = new List<int>() { 2,4,5 };
        }
    }
}
