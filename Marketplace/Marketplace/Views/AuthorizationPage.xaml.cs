using Marketplace.Models;
using Marketplace.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            string email = Preferences.Get("UserEmail", "null");
            string password = Preferences.Get("UserPassword", "null");
            if (email != "null")
            {
                //Context.UserNow = await Context.Api.Authorize(login, password);
                //Context.Api.Jwt = Context.UserNow.Token;
            }
        }

        async private void BtnSignIn_Clicked(object sender, EventArgs e)
        {
            var result = string.Empty;
            string ValidateFields()
            {
                if (string.IsNullOrWhiteSpace(tbEmail.Text))
                    result += "Введите почту!\n";

                if (string.IsNullOrWhiteSpace(tbPassword.Text))
                    result += "Введите пароль!\n";

                return result;
            }

            if (!string.IsNullOrEmpty(ValidateFields()))
            {
                await DisplayAlert("Ошибка", ValidateFields(), "ОК");
                return;
            }


            using (var httpClient = new HttpClient())
            {
               
                string apiUrl = $"{Context.host}/api/user/auth?email={tbEmail.Text}&password={tbPassword.Text}";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    User userReresponse = JsonConvert.DeserializeObject<User>(responseBody);
                    Context.CurrentUser = userReresponse;

                    Preferences.Set("UserEmail", tbEmail.Text);
                    Preferences.Set("UserPassword", tbPassword.Text);
                    Application.Current.MainPage = new AppShell();
                    return;
                }
                else
                {
                    await DisplayAlert("Ошибка при выполнении запроса.", "Код статуса: " + response.StatusCode, "ОК");
                }
            }

        }

        async private void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}