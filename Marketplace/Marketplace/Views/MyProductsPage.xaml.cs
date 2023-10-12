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
        }

        private async void CvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                //Context.AdNow = (Ad)e.CurrentSelection.FirstOrDefault();
                //Context.AdNow = await Context.Api.GetAd(Context.AdNow.Id);
                //await Shell.Current.GoToAsync(nameof(AdPage));
            }
        }

        private async void btnDeleteFromBasket_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BindingContext is Test data)
                {
                    await DisplayAlert("Успешно", $"Id элемента {data.Id}", "ОК");
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