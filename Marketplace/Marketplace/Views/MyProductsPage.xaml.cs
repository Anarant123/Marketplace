using Marketplace.Models.db;
using Marketplace.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProductsPage : ContentPage
    {
        public MyProductsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Context.ProductsList = await GetProductsAsync();
            cvProduct.ItemsSource = Context.ProductsList;
            if (Context.ProductsList.Any())
                lbMassage.IsVisible = false;
            else
                lbMassage.IsVisible = true;
        }

        private async void CvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Context.CurrentProduct = (Product)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync(nameof(ProductInfoPage));
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BindingContext is Product product)
                {
                    using (var httpClient = new HttpClient())
                    {

                        string apiUrl = $"{Context.host}/api/product/deletebyid/{product.ProductId}";
                        HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);

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

        private async Task<List<Product>> GetProductsAsync()
        {
            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/user/products?id={Context.CurrentUser.UserId}";
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