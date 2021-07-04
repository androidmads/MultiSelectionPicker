using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CheckboxPage : BasePage<string>
{
    private List<CheckedItem> ItemsSource;
    public CheckboxPage(List<string> itemsSource, List<int> selectedIndices)
    {
        InitializeComponent();
        List<CheckedItem> list = new List<CheckedItem>();
        for (int i = 0; i < itemsSource.Count; i++)
        {
            list.Add(new CheckedItem()
            {
                Text = itemsSource[i],
                IsChecked = false,
                Position = i
            });
        }
        ItemsSource = list;
        foreach (int i in selectedIndices)
        {
            list[i].IsChecked = true;
        }
        listView.ItemsSource = list;
    }

    private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }

    private void Done_Clicked(object sender, EventArgs e)
    {
        List<CheckedItem> list = (List<CheckedItem>)listView.ItemsSource;
        _navigationResut = string.Join(",", list.Where(x => x.IsChecked).Select(x => x.Position).ToArray());
        Navigation.PopModalAsync();
    }

    private void Cancel_Clicked(object sender, EventArgs e)
    {
        //List<CheckedItem> list = ItemsSource;
        _navigationResut = "";// string.Join(",", list.Where(x => x.IsChecked).Select(x => x.Position).ToArray());
        Navigation.PopModalAsync();
    }
}

public class CheckedItem
{
    public int Position { get; set; }
    public bool IsChecked { get; set; }
    public string Text { get; set; }
}
}