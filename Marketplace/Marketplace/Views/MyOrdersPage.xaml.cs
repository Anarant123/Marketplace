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
    public partial class MyOrdersPage : ContentPage
    {
        public MyOrdersPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Context.OrdersList = await GetOrdersAsync();
            cvOrders.ItemsSource = Context.OrdersList;
            if (Context.OrdersList.Any())
                lbBasketMassage.IsVisible = false;
            else
                lbBasketMassage.IsVisible = true;
        }

        private async void CvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                //Context.CurrentProduct = (Product)e.CurrentSelection.FirstOrDefault();
                //await Shell.Current.GoToAsync(nameof(ProductInfoPage));
            }
        }

        private async void btnCancelOrder_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BindingContext is Order order)
                {
                    using (var httpClient = new HttpClient())
                    {

                        string apiUrl = $"{Context.host}/api/order/deletebyid/{order.OrderId}";
                        HttpResponseMessage response = await httpClient.DeleteAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("Успешно", "Вы отменили заказ", "ОК");
                            OnAppearing();
                        }
                        else
                        {
                            await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                        }
                    }
                }
            }
        }

        private async Task<List<Order>> GetOrdersAsync()
        {
            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/order/getall?userId={Context.CurrentUser.UserId}";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                List<Order> list = new List<Order>();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<Order>>(responseBody);
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