using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            if (tbEmail.Text == null || tbPassword.Text == null)
            {
                await DisplayAlert("Ошибка", "Заполните поля", "ОК");
                return;
            }
            //Context.UserNow = await Context.Api.Authorize(tbLogin.Text, tbPassword.Text);
            //Context.Api.Jwt = Context.UserNow.Token;

            //if (Context.UserNow == null)
            //{
            //    await DisplayAlert("Ошибка", "Вы ввели неверные данные", "ОК");
            //    return;
            //}

            Preferences.Set("UserEmail", tbEmail.Text);
            Preferences.Set("UserPassword", tbPassword.Text);
            Application.Current.MainPage = new AppShell();
        }

        async private void BtnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}