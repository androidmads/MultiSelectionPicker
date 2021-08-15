using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms
{
    public class MultiSelectionPicker : Entry
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(MultiSelectionPicker), null);
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(List<string>), typeof(MultiSelectionPicker), null, BindingMode.TwoWay);
        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly BindableProperty SelectedIndicesProperty = BindableProperty.Create("SelectedItems", typeof(List<int>), typeof(MultiSelectionPicker), null, BindingMode.TwoWay,
            propertyChanged: SelectedIndexChanged);

        public List<int> SelectedIndices
        {
            get { return (List<int>)GetValue(SelectedIndicesProperty); }
            set { SetValue(SelectedIndicesProperty, value); }
        }

        private static void SelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ctrl = (MultiSelectionPicker)bindable;
            if (ctrl == null)
                return;
            List<string> selectedItems = new List<string>();
            foreach (int i in ctrl.SelectedIndices)
            {
                selectedItems.Add(ctrl.ItemsSource[i]);
            }
            ctrl.Text = string.Join(", ", selectedItems);
        }

        bool IsControlLoaded = false;

        public MultiSelectionPicker()
        {
            Focused += async (e, s) =>
            {
                if (!IsControlLoaded && Device.RuntimePlatform == Device.UWP)
                {
                    IsControlLoaded = true;
                    Unfocus();
                    return;
                }
                if (s.IsFocused)
                {
                    Unfocus();
                    string item = await NavigateToModal<string>(new CheckboxPage(ItemsSource, SelectedIndices));
                    if (item == ""){
                        SelectedIndices = new List<int>();
                        Text = "";
                    }else{
                        SelectedIndices = item.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        List<string> selectedItems = new List<string>();
                        foreach (int i in SelectedIndices)
                        {
                            selectedItems.Add(ItemsSource[i]);
                        }
                        Text = string.Join(", ", selectedItems);
                    }
                }
            };
        }

        public async Task<T> NavigateToModal<T>(BasePage<T> page)
        {
            var source = new TaskCompletionSource<T>();
            page.PageDisappearing += (result) =>
            {
                var res = (T)Convert.ChangeType(result, typeof(T));
                source.SetResult(res);
            };
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page));
            return await source.Task;
        }
    }

    public class BasePage<T> : ContentPage
    {
        public event Action<T> PageDisappearing;
        protected T _navigationResut;

        public BasePage()
        {

        }

        protected override void OnDisappearing()
        {
            PageDisappearing?.Invoke(_navigationResut);
            if (PageDisappearing != null)
            {
                foreach (var @delegate in PageDisappearing.GetInvocationList())
                {
                    PageDisappearing -= @delegate as Action<T>;
                }
            }
            base.OnDisappearing();
        }
    }
}
