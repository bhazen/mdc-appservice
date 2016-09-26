using System;
using System.Threading.Tasks;
using AppServiceMDC.XamarinForms.ViewModels;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.Pages
{
    public partial class Expenses : ContentPage
    {
        private ExpensesViewModel _vm;

        public Expenses()
        {
            InitializeComponent();
            BindingContext = _vm = new ExpensesViewModel();
            ListViewExpenses.ItemTapped += (sender, e) => ListViewExpenses.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
            OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;
            if (AppServiceMDC.XamarinForms.Helpers.Settings.IsLoggedIn)
            {
                try
                {
                    Task.Run(() => _vm.LoadExpensesCommand.Execute(null)).Wait();
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                }
            }
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossConnectivity.Current.ConnectivityChanged -= ConnecitvityChanged;
        }

        private void ConnecitvityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                OfflineStack.IsVisible = !e.IsConnected;
            });
        }
    }
}
