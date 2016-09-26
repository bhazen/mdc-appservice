using System;
using System.Threading.Tasks;
using AppServiceMDC.XamarinForms.Droid.Helpers;
using AppServiceMDC.XamarinForms.Helpers;
using AppServiceMDC.XamarinForms.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(Authentication))]
namespace AppServiceMDC.XamarinForms.Droid.Helpers
{
    public class Authentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {
                var user = await client.LoginAsync(Forms.Context, provider);
                
                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId ?? string.Empty;

                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error authenticating {ex}");   
            }

            return null;
        }

        public void ClearCookies()
        {
            try
            {
                if ((int) global::Android.OS.Build.VERSION.SdkInt >= 21)
                {
                    global::Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing cookies {ex}");   
            }
        }
    }
}