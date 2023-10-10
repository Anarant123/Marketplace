using Marketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

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
            DateTime currentDate = DateTime.Now;

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

            //PersonReg person = new()
            //{
            //    Login = tbLogin.Text,
            //    Birthday = dpBirthday.Date,
            //    Phone = tbPhone.Text,
            //    Email = tbEmail.Text,
            //    Password = tbPassword1.Text,
            //    ConfirmPassword = tbPassword2.Text
            //};

            //var result = await Context.Api.Registr(person);


            //if (result)
            //{
                await Navigation.PopAsync();
                return;
            //}
            await DisplayAlert("Ошибка", "Пользователь с данным логином или Email уже существует", "ОК");

        }
    }
}