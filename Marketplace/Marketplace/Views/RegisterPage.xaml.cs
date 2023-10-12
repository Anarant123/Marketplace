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
            if (tbPassword1.Text != tbPassword2.Text)
            {
                await DisplayAlert("Ошибка", "Пароли должны совпадать!", "ОК");
                return;
            }

            if (tbFirstName.Text == "" || tbLastName.Text == "" || tbPhone.Text == "" || tbEmail.Text == "" || tbPassword1.Text == "" || tbImage.Text == "")
            {
                await DisplayAlert("Ошибка", "Заполните все поля!", "ОК");
                return;
            }

            using (var httpClient = new HttpClient())
            {
                string apiUrl = $"{Context.host}/api/user/create";

                User user = new User(tbFirstName.Text, tbLastName.Text, tbEmail.Text, tbPassword1.Text, tbPhone.Text, tbImage.Text);
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