using AppServiceMDC.XamarinForms.Pages;
using FormsToolkit;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms
{
    public class App : Application
    {
        public App()
        {
            MainPage = new EntryPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MessagingService.Current.Subscribe<MessagingServiceAlert>("message", async (m, info) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);

                if (task == null)
                {
                    return;
                }

                await task;
                info?.OnCompleted?.Invoke();
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}