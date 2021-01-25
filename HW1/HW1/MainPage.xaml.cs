using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HW1
{
    public partial class MainPage : ContentPage
    {
        private readonly List<Product> _products;

        public MainPage()
        {
            InitializeComponent();
            // загрузка из json
            var assembly = GetType().Assembly;
            Stream stream = assembly.GetManifestResourceStream("HW1.products.json");
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            _products = JsonConvert.DeserializeObject<List<Product>>(json);

            ProductPicker.ItemsSource = _products.ConvertAll(a => a.Name);

            Task.Run(AnimateGrad);
        }

        private void ProductPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is Picker picker)) return;

            var key = picker.SelectedItem?.ToString();
            DescriptionLabel.Text = _products[picker.SelectedIndex].Description;
            ProductImage.Source = ImageSource.FromFile(_products[picker.SelectedIndex].PicPath);

            // TODO возможно это не нужно
            if (Cart.Products.ContainsKey(key))
            {
                var count = Cart.Products[key].Count;
                AmountProduct.Text = count.ToString();
                StepperProduct.Value = count;
            }
            else
            {
                AmountProduct.Text = "0";
                StepperProduct.Value = 0;
            }
        }

        private void AmountProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AmountProduct.Text.Length == 0) return;
            var count = int.Parse(AmountProduct.Text);
            if (count > 100)
            {
                count = 100;
                AmountProduct.Text = count.ToString();
            }
            StepperProduct.Value = count;
        }

        private void StepperProduct_ValueChanged(object sender, ValueChangedEventArgs e) =>
            AmountProduct.Text = (sender as Stepper)?.Value.ToString();

        private void Order_Clicked(object sender, EventArgs e)
        {
            // TODO добавление продукта в корзину
            var key = ProductPicker.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(key))
            {
                DisplayAlert("Order", "The product was not selected", "Ok");
                return;
            }
            if (int.Parse(AmountProduct.Text) == 0) return;

            if (!Cart.Products.ContainsKey(key))
            {
                Cart.Products[key] = new OrderProduct
                {
                    Name = ProductPicker.SelectedItem?.ToString(),
                    Count = int.Parse(AmountProduct.Text),
                    PicPath = _products[ProductPicker.SelectedIndex].PicPath
                };
            }
            else
            {
                Cart.Products[key].Count += int.Parse(AmountProduct.Text);
            }

            DisplayAlert("Order", "Successful", "Ok");
        }

        private void Cart_Clicked(object sender, EventArgs e)
        {
            if (!(sender is Button button)) return;
            button.IsEnabled = false;

            var cart = new Cart();
            cart.Disappearing += (s, ev) => button.IsEnabled = true;

            Navigation.PushAsync(cart);
        }

        private async void AnimateGrad()
        {
            Action<double> forward = input => gradBox.AnchorX = input;
            Action<double> backward = input => gradBox.AnchorX = input;

            while (true)
            {
                gradBox.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 6000, easing: Easing.SinIn);
                await Task.Delay(6000);
                gradBox.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 6000, easing: Easing.SinIn);
                await Task.Delay(6000);
            }
        }
    }
}