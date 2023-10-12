using Marketplace.Models.db;
using Marketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void BtnRegistration_Clicked(object sender, EventArgs e)
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
                string apiUrl = $"{Context.host}/api/user/create";

                User user = new User 
                {
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    Email = tbEmail.Text,
                    PasswordHash = tbPassword1.Text,
                    Phone = tbPhone.Text,
                    ImageUrl = tbImage.Text,
                };
                string jsonUser = JsonSerializer.Serialize(user);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {

                    await DisplayAlert("Успешно", "Пользователь зарегистрирован", "ОК");
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