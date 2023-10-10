using Marketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Marketplace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSearchPage : ContentPage
    {
        public ProductSearchPage()
        {
            InitializeComponent();
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            filterContainer.IsVisible = !filterContainer.IsVisible;
        }

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            //cvProduct.ItemsSource = await Context.Api.UseFulter(1, tbPriceFrom.Text, tbPriceTo.Text, tbCity.Text, Convert.ToInt32(pickerCategory.SelectedIndex), (bool)rbBuy.IsChecked!, (bool)rbSell.IsChecked!);

            //if ((cvProduct.ItemsSource as List<Ad>).Count == 0)
            //    await DisplayAlert("Сообщение об ошибке", "С данными фильтрами ничего не найденно", "OK");
        }
    }
}