using Marketplace.Models;
using Marketplace.Models.db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;
using static Xamarin.Essentials.Permissions;

namespace Marketplace.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketPage : ContentPage
    {

        public BasketPage()
        {

            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Context.ProductsList = await GetProductsAsync();
            cvProduct.ItemsSource = Context.ProductsList;
            if (Context.ProductsList.Any())
                lbBasketMassage.IsVisible = false;
            else
                lbBasketMassage.IsVisible = true;
        }

        private async void CvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cvProduct.SelectedItem is Product selectedProduct)
            {
                using (var httpClient = new HttpClient())
                {

                    string apiUrl = $"{Context.host}/api/product/getbyid/{selectedProduct.ProductId}";
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Context.CurrentProduct = JsonConvert.DeserializeObject<Product>(responseBody);

                        await Shell.Current.GoToAsync(nameof(ProductInfoPage));
                        return;
                    }
                    else
                    {
                        await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                    }
                }
            }
        }

        private async void btnDeleteFromBasket_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BindingContext is Product product)
                {
                    using (var httpClient = new HttpClient())
                    {

                        string apiUrl = $"{Context.host}/api/cart/remove-product?userId={Context.CurrentUser.UserId}&productId={product.ProductId}";
                        HttpResponseMessage response = await httpClient.PostAsync(apiUrl, null);

                        if (response.IsSuccessStatusCode)
                        {
                            Context.ProductsList = await GetProductsAsync();
                            cvProduct.ItemsSource = Context.ProductsList;
                        }
                        else
                        {
                            await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                        }
                    }
                }
            }
        }

        private async void btnСheckout_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                string result = await DisplayPromptAsync("Укажите количество", "", "ОК", "Отмена", "Количество", 3, Keyboard.Default, "");

                if (result == null)
                    return;

                var count = int.Parse(result);

                if (button.BindingContext is Product product)
                {
                    using (var httpClient = new HttpClient())
                    {

                        string apiUrl = $"{Context.host}/api/order/create";

                        Order order = new Order()
                        {
                            TotalQuantity = count,
                            TotalAmount = product.Price * count,
                            UserId = Context.CurrentUser.UserId,
                            ProductId = product.ProductId,
                        };
                        string jsonUser = JsonConvert.SerializeObject(order);
                        var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                        var response = await httpClient.PutAsync(apiUrl, content);

                        if (response.IsSuccessStatusCode)
                        {

                            await DisplayAlert("Успешно", "Заказ оформлен", "ОК");
                            await Navigation.PopAsync();
                            return;
                        }
                        else
                        {
                            await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                        }
                    }
                }
            }
        }

        private async Task<List<Product>> GetProductsAsync()
        {
            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/cart?userId={Context.CurrentUser.UserId}";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                List<Product> list = new List<Product>();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Product>>(responseBody);
                    return list;
                }
                else
                {
                    await DisplayAlert("Очень жаль", "Товары отстутствуют", "ОК");
                    return list;
                }
            }
        }
    }
}