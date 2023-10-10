using Marketplace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateProductPage : ContentPage
    {
        //readonly AddAdModel ad = new();
        public CreateProductPage()
        {
            InitializeComponent();

            pickerCategory.SelectedIndex = 0;

        }

        private async void BtnGetPhoto_Clicked(object sender, EventArgs e)
        {
            //Фотка по URL намутить
        }

        async private void BtnCreateAd_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbName.Text) || string.IsNullOrEmpty(tbPrice.Text) || string.IsNullOrEmpty(tbDescription.Text) || string.IsNullOrEmpty(tbStockQuantity.Text))
            {
                await DisplayAlert("Ошибка", "Заполните все поля!", "ОК");
                return;
            }



            //ad.Name = tbName.Text;
            //ad.City = tbCity.Text;
            //ad.CategoryId = pickerCategory.SelectedIndex + 1;
            //ad.Description = tbDescription.Text;
            //ad.Price = Convert.ToInt32(tbPrice.Text);
            //ad.AdTypeId = rbBuy.IsChecked ? 1 : 2;

            //ad.Id = (await Context.Api.AddAd(ad)).Id;
            //Context.AdNow = await Context.Api.UpdateAdPhoto(ad);

            //if (Context.AdNow == null)
            //{
            //    await DisplayAlert("Ошибка", "Что то пошло не так! \nОбъявление добавить не удалось...", "ОК");
            //    return;
            //}
            await DisplayAlert("Успешно", "Вы добавили объявление", "ОК");
            await Shell.Current.GoToAsync(nameof(ProductInfoPage));
        }
    }
}