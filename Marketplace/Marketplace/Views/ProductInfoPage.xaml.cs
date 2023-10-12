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
using static Xamarin.Essentials.Permissions;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductInfoPage : ContentPage
    {
        public ProductInfoPage()
        {
            InitializeComponent();

            lbName.Text = Context.CurrentProduct.Name;
            lbDescription.Text = Context.CurrentProduct.Description;
            lbPrice.Text = Context.CurrentProduct.Price.ToString();
            lbStockQuantity.Text = Context.CurrentProduct.StockQuantity.ToString();
            lbUserSeller.Text = Context.CurrentProduct.Seller.FirstName + " " + Context.CurrentProduct.Seller.LastName;
            imgProduct.Source = Context.CurrentProduct.ImageUrl;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            cvComments.ItemsSource = await GetReviewsAsync();
        }

        private async void BtnAddToBasket_Clicked(object sender, EventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"{Context.host}/api/cart/add-product?userId={Context.CurrentUser.UserId}&productId={Context.CurrentProduct.ProductId}";

                var response = await httpClient.PostAsync(apiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Успешно", "Товар добавлен в корзину", "ОК");
                    return;
                }
                else
                {
                    await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                }
            }
        }

        private async void btnSubmitComment_Clicked(object sender, EventArgs e)
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"{Context.host}/api/review/create";

                Review review = new Review()
                {
                    Rating = Convert.ToInt32(header.Text),
                    Comment = tbComment.Text,
                    CreatedAt = DateTime.Now,
                    ImageUrl = tbCommentImg.Text,
                    UserId = Context.CurrentUser.UserId,
                    ProductId = Context.CurrentProduct.ProductId,
                };

                string jsonReview = JsonConvert.SerializeObject(review);
                var content = new StringContent(jsonReview, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {

                    await DisplayAlert("Успешно", "Спасибо за коментарий", "ОК");
                    return;
                }
                else
                {
                    await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                }
            }
            cvComments.ItemsSource = await GetReviewsAsync();
        }

        private async Task<List<Review>> GetReviewsAsync()
        {
            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/product/{Context.CurrentProduct.ProductId}/reviews";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                List<Review> list = new List<Review>();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Review>>(responseBody);
                }
                return list;
            }
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double newValue = Math.Round(e.NewValue);
            (sender as Slider).Value = newValue;
            header.Text = newValue.ToString();
        }
    }
}