using Marketplace.Models;
using Marketplace.Models.db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var person = await getUserInfo();

            tbFirstName.Text = person.FirstName;
            tbLastName.Text = person.LastName;
            tbEmail.Text = person.Email;
            tbPassword1.Text = person.PasswordHash;
            tbPassword2.Text = person.PasswordHash;
            tbPhone.Text = person.Phone;
            tbImgUrl.Text = person.ImageUrl;
        }

        async private void BtnSaveChanges_Clicked(object sender, EventArgs e)
        {
            string ValidateFields()
            {
                string result = string.Empty;
                if (string.IsNullOrWhiteSpace(tbFirstName.Text))
                    result += "Введите имя.\n";

                if (string.IsNullOrWhiteSpace(tbLastName.Text))
                    result += "Введите фамилию.\n";

                if (string.IsNullOrWhiteSpace(tbEmail.Text) || !IsValidEmail(tbEmail.Text))
                    result += "Введите корректный email.\n";

                if (string.IsNullOrWhiteSpace(tbPhone.Text) || !IsValidPhone(tbPhone.Text))
                    result += "Введите корректный номер телефона.\n";

                if (string.IsNullOrWhiteSpace(tbPassword1.Text) || string.IsNullOrWhiteSpace(tbPassword2.Text) || tbPassword1.Text.Length < 8)
                    result += "Введите пароль в оба поля.";
                else if (tbPassword1.Text != tbPassword2.Text)
                    result += "Пароли не совпадают.\n";

                return result;
            }

            bool IsValidEmail(string email)
            {
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
                Match match = Regex.Match(email, pattern);
                return match.Success;
            }

            bool IsValidPhone(string phone)
            {
                string pattern = @"^(\+)[1-9][0-9\-().]{9,15}$";
                Match match = Regex.Match(phone, pattern);
                return match.Success;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                await DisplayAlert("Ошибка", ValidateFields(), "ОК");
                return;
            }

            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"{Context.host}/api/user/update";

                User user = new User
                {
                    UserId = Context.CurrentUser.UserId,
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    Email = tbEmail.Text,
                    PasswordHash = tbPassword1.Text,
                    Phone = tbPhone.Text,
                    ImageUrl = tbImgUrl.Text,
                };
                string jsonUser = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Context.CurrentUser = JsonConvert.DeserializeObject<User>(responseBody);
                    await DisplayAlert("Успешно", "Данные о пользователе обновленны", "ОК");
                    return;
                }
                else
                {
                    await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                }
            }
        }

        private async void btnShowMyOrders_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MyOrdersPage));
        }

        private async Task<User> getUserInfo()
        {

            using (var httpClient = new HttpClient())
            {

                string apiUrl = $"{Context.host}/api/user/getbyid/{Context.CurrentUser.UserId}";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    User userReresponse = JsonConvert.DeserializeObject<User>(responseBody);
                    Context.CurrentUser = userReresponse;

                    return userReresponse;
                }
                else
                {
                    await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                    return null;
                }
            }
        }

        private void tbImgUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            imgUser.Source = tbImgUrl.Text;
        }
    }
}