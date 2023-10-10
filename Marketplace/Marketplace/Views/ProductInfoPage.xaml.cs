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
    public partial class ProductInfoPage : ContentPage
    {
        bool isF;
        public ProductInfoPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //lbName.Text = Context.AdNow.Name;
            //lbDescription.Text = Context.AdNow.Description;
            //lbCity.Text = Context.AdNow.City;
            //lbPhone.Text = Context.AdNow.Person.Phone;
            //lbPrice.Text = Context.AdNow.Price.ToString();
            //lbSalesman.Text = Context.AdNow.Person.Name;
            //imgAd.Source = Context.AdNow.PhotoName;
            //imgPerson.Source = Context.AdNow.Person.PhotoName;

            //isF = Context.AdNow.IsFavorite;
            //if (isF)
            //    btnAddToFavorites.Text = "Удалить из избранного";
        }

        private async void BtnAddToBasket_Clicked(object sender, EventArgs e)
        {
            if (isF)
            {
                //var result = await Context.Api.DeleteFromFavorites(Context.AdNow.Id);
                //if (!result)
                //{
                //    await DisplayAlert("Ошибка", "Что то пошло не так...", "ОК");
                //    return;
                //}

                //await DisplayAlert("Успешно", "Объявление удалено из избранного", "ОК");
                //btnAddToFavorites.Text = "Добавить в избранное";
                //isF = false;
            }
            else
            {
                //var result = await Context.Api.AddToFavorites(Context.AdNow.Id);

                //if (!result)
                //{
                //    await DisplayAlert("Ошибка", "Что то пошло не так...", "ОК");
                //    return;
                //}
                //await DisplayAlert("Успешно", "Объявление добавленно в избранное", "ОК");
                //btnAddToFavorites.Text = "Удалить из избранного";
                //isF = true;
            }
        }

        private void btnSubmitComment_Clicked(object sender, EventArgs e)
        {

        }
    }
}