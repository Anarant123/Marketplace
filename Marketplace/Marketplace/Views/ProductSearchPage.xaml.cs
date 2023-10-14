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

            pickerCategory.SelectedIndex = 0;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Context.ProductsList = await GetProductsAsync();
            cvProduct.ItemsSource = Context.ProductsList;
            pickerCategory.ItemsSource = await SetCategory();
            pickerCategory.SelectedIndex = 0;
        }

        private async void CvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Context.CurrentProduct = (Product)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync(nameof(ProductInfoPage));
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            filterContainer.IsVisible = !filterContainer.IsVisible;
        }

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            var list = await GetProductsAsync();
            list = list.Where(x => (pickerCategory.SelectedIndex == 0 || x.CategoryId == pickerCategory.SelectedIndex) && x.Name.Contains(tbName.Text)).ToList();

            cvProduct.ItemsSource = list;
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

        private async Task<List<string>> SetCategory()
        {
            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/category/getall";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                List<Category> list = new List<Category>();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Category>>(responseBody);
                }
                else
                    await DisplayAlert("Очень жаль", "Товары отстутствуют", "ОК");

                var listCategories = new List<string>() { "Категория" };
                foreach (var category in list)
                {
                    listCategories.Add(category.Name);
                }
                return listCategories;

            }
        }
    }
}