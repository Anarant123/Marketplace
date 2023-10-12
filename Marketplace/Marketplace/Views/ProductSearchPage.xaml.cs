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
using Xamarin.Forms.Xaml;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSearchPage : ContentPage
    {
        public ProductSearchPage()
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            filterContainer.IsVisible = !filterContainer.IsVisible;
        }

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            //cvProduct.ItemsSource = await Context.Api.UseFulter(1, tbPriceFrom.Text, tbPriceTo.Text, tbCity.Text, Convert.ToInt32(pickerCategory.SelectedIndex), (bool)rbBuy.IsChecked!, (bool)rbSell.IsChecked!);

            //if ((cvProduct.ItemsSource as List<Ad>).Count == 0)
            //    await DisplayAlert("Сообщение об ошибке", "С данными фильтрами ничего не найденно", "OK");
        }

        private async Task<List<Product>> GetProductsAsync()
        {
            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/product/getall";
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