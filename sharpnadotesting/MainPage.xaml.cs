using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace sharpnadotesting
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {
        public int ItemIndex { get; set; } 
        public ObservableCollection<Item> Items { get; set; } 
        public string SelectedItemText { get; set; }

        public MainViewModel()
        {
            Items = new ObservableCollection<Item>();
            Items.Add(new Item { ItemId = 1, ItemName = "Item 1" });
            Items.Add(new Item { ItemId = 2, ItemName = "Item 2" });
            Items.Add(new Item { ItemId = 3, ItemName = "Item 3" });
        }

        void OnItemIndexChanged()
        {
            if (ItemIndex < 0)
            { 
                SelectedItemText = $"ItemIndex: {ItemIndex}";
                return;
			}
            var item = Items[ItemIndex];
            SelectedItemText = $"ItemIndex: {ItemIndex}, Item:{item.ItemId}:{item.ItemName}";
		}
        public Command AddItem => new Command(() =>
        {
            int id = 0;
            var last = Items.LastOrDefault();
            if (last != null)
                id = last.ItemId + 1;
            else
                id = 1;

            Items.Add(new Item { ItemId = id, ItemName = $"Item {id}" });
        });
        public Command Clear => new Command(() =>
        {
            Items.Clear();
            ItemIndex = -1;
            return;
        });
        public Command Deselect => new Command(() =>
        {
            ItemIndex = -1;
        });
        public Command SelectNext => new Command(() =>
        {
            if (ItemIndex < Items.Count - 1)
                ItemIndex++;
            else
                ItemIndex = 0;
        });
        public Command RemoveItem => new Command(() =>
        {
            if (Items.Count == 0) return;
            Item removeItem = null;
            int priorIndex = ItemIndex >= 0 ? ItemIndex : -1;
            if (ItemIndex == -1)
                removeItem = Items.Last();
            else
                removeItem = Items[ItemIndex];
            Items.Remove(removeItem);
            if (priorIndex >= 1)
                ItemIndex = priorIndex - 1;
        });
	}

    public class Item
    { 
        public int ItemId { get; set; }
        public string ItemName { get; set; }
	}
}
