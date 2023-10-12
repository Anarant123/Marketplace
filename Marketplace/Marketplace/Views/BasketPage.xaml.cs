﻿using Marketplace.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace Marketplace.Views
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasketPage : ContentPage
    {
        Test product = new Test()
        {
            Id = 1,
            Name = "John",
            Price = 500,
        };
        public BasketPage()
        {

            InitializeComponent();
            cvProduct.ItemsSource = new List<Test>() { product };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Context.AdList = new AdListViewModel
            //{
            //    Ads = await Context.Api.GetAds()
            //};
            //cvAds.ItemsSource = Context.AdList.Ads.ToList();
        }

        private async void CvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                //Context.AdNow = (Ad)e.CurrentSelection.FirstOrDefault();
                //Context.AdNow = await Context.Api.GetAd(Context.AdNow.Id);
                //await Shell.Current.GoToAsync(nameof(AdPage));
            }
        }

        private async void btnDeleteFromBasket_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.BindingContext is Test data)
                {
                    await DisplayAlert("Успешно", $"Id элемента {data.Id}", "ОК");
                }
            }
        }
    }
}