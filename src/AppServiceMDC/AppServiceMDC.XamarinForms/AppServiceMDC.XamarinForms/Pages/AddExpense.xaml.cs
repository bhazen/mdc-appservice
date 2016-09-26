using System;
using System.Linq;
using AppServiceMDC.XamarinForms.ViewModels;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.Pages
{
    public partial class AddExpense : ContentPage
    {
        public AddExpense()
        {
            InitializeComponent();
            var uploadContenxt = new AddExpenseViewModel();
            BindingContext = uploadContenxt;

            Date.MinimumDate = new DateTime(DateTime.Now.Year, 1, 1);
            Date.Date = DateTime.Today;
            Date.MaximumDate = DateTime.UtcNow;

            foreach (var category in uploadContenxt.Categories.Select(c => c.Name))
            {
                CategoryPicker.Items.Add(category);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        }

        private void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}