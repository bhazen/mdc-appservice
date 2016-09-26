using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AppServiceMDC.XamarinForms.Interfaces;
using AppServiceMDC.XamarinForms.Pages;
using AppServiceMDC.XamarinForms.Services;
using Microsoft.WindowsAzure.MobileServices;
using MvvmHelpers;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.ViewModels
{
    public class EntryPageViewModel : BaseViewModel
    {
        private readonly AzureService _azureService;

        public EntryPageViewModel()
        {
            Title = "Login";
            _azureService = new AzureService();
        }

        private Command _loginCommand;

        public Command LoginCommand
            => _loginCommand ?? (_loginCommand = new Command(async () => await ExecuteLoginCommand()));

        private async Task ExecuteLoginCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                await _azureService.Initialize();
                var user = await DependencyService.Get<IAuthentication>().LoginAsync(_azureService.MobileService, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                if (user == null)
                {
                    MessagingCenter.Send(this, "Unable to authenticate");
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new Expenses());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error = {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}