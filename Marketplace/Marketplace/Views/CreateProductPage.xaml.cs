using Marketplace.Models;
using Marketplace.Models.db;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateProductPage : ContentPage
    {
        public CreateProductPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            pickerCategory.ItemsSource = await SetCategory();
            pickerCategory.SelectedIndex = 0;
        }

        async private void BtnCreateAd_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text) ||
                string.IsNullOrEmpty(tbPrice.Text) ||
                string.IsNullOrEmpty(tbDescription.Text) ||
                string.IsNullOrEmpty(tbStockQuantity.Text) ||
                pickerCategory.SelectedIndex == 0)
            {
                await DisplayAlert("Ошибка", "Заполните все поля!", "ОК");
                return;
            }


            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"{Context.host}/api/product/create";

                Product product = new Product()
                {
                    Name = tbName.Text,
                    Description = tbDescription.Text,
                    Price = decimal.Parse(tbPrice.Text),
                    StockQuantity = int.Parse(tbStockQuantity.Text),
                    ImageUrl = tbImgUrl.Text,
                    CategoryId = pickerCategory.SelectedIndex,
                    SellerId = Context.CurrentUser.UserId,
                };
                string jsonUser = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {

                    await DisplayAlert("Успешно", "Вы отправили заявку на добавление товара", "ОК");
                    return;
                }
                else
                {
                    await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                }
            }
        }

        private void tbImgUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            imgProduct.Source = tbImgUrl.Text;
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

                var listCategories = new List<string>(){"Категория"};
                foreach (var category in list)
                {
                    listCategories.Add(category.Name);
                }
                return listCategories;

            }
        }

        private void tbStockQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = tbPrice.Text;
            string digitsOnly = new string(text.Where(char.IsDigit).ToArray());

            if (!string.IsNullOrEmpty(digitsOnly))
            {
                int value = int.Parse(digitsOnly);
                if (value < 0)
                {
                    value = Math.Abs(value);
                    digitsOnly = value.ToString();
                }
            }

            tbPrice.Text = digitsOnly;
        }
    }
}