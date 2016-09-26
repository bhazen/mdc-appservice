using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace AppServiceMDC.XamarinForms.Interfaces
{
    public interface IAuthentication
    {
        Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider);
        void ClearCookies();
    }
}