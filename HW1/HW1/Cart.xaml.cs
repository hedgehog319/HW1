using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HW1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cart : ContentPage
    {
        public static Dictionary<string, OrderProduct> Products { get; set; } = new Dictionary<string, OrderProduct>();

        public Cart()
        {
            InitializeComponent();
            GoodsListView.ItemsSource = Products.Select(a => a.Value).ToList();
        }

        private async void OrderClick(object sender, EventArgs e)
        {
            var list = Products.Select(a => a.Value).ToList();
            var json = JsonConvert.SerializeObject(list);

            if (await DisplayAlert("Order", json, "Yes", "No"))
            {
                await DisplayAlert("Order", "Eeee boy!", "Ok");
                Products.Clear();
                await Navigation.PopAsync();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (!(sender is Button item)) return;

            var itemsList = (from itm in Products
                             where itm.Value.Name == item.CommandParameter.ToString()
                             select itm).First();

            Products.Remove(itemsList.Key);
            GoodsListView.ItemsSource = Products.Select(a => a.Value).ToList();
        }

        private void Subtract_Click(object sender, EventArgs e)
        {
            if (!(sender is Button item)) return;

            var key = item.CommandParameter.ToString();
            var selItem = Products.First(a => a.Value.Name == key);

            selItem.Value.Count--;

            if (selItem.Value.Count == 0)
            {
                Products.Remove(selItem.Key);
            }

            GoodsListView.ItemsSource = Products.Select(a => a.Value).ToList();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (!(sender is Button item)) return;

            var key = item.CommandParameter.ToString();
            var selItem = Products.First(a => a.Value.Name == key);

            if (selItem.Value.Count + 1 <= 100)
                selItem.Value.Count++;

            GoodsListView.ItemsSource = Products.Select(a => a.Value).ToList();
        }
    }
}