using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppServiceMDC.XamarinForms.Helpers;
using AppServiceMDC.XamarinForms.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.Services
{
    class AuthHandler : DelegatingHandler
    {
        public IMobileServiceClient Client { get; set; }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (Client == null)
            {
                throw new InvalidOperationException("Make sure to set the 'Client' property in  this handler before using it");
            }

            var clonedRequest = await CloneRequest(request);
            var response = await base.SendAsync(clonedRequest, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    //var user = await Client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, null);
                    var user = await DependencyService.Get<IAuthentication>().LoginAsync((MobileServiceClient)Client, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

                    clonedRequest = await CloneRequest(request);
                    Settings.UserId = user.UserId;
                    Settings.AuthToken = user.MobileServiceAuthenticationToken;

                    clonedRequest.Headers.Remove("X-ZUMO-AUTH");
                    clonedRequest.Headers.Add("X-ZUMO-AUTH", user.MobileServiceAuthenticationToken);

                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
                catch (InvalidOperationException)
                {
                    var blah = response.Content.ReadAsStringAsync().Result;
                    return response;
                }
            }

            var test = response.Content.ReadAsStringAsync().Result;

            return response;
        }

        private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
        {
            var result = new HttpRequestMessage(request.Method, request.RequestUri);
            foreach (var header in request.Headers)
            {
                result.Headers.Add(header.Key, header.Value);
            }

            if (request.Content != null && request.Content.Headers.ContentType != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                var mediaType = request.Content.Headers.ContentType.MediaType;
                result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
                foreach (var header in request.Content.Headers)
                {
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Content.Headers.Add(header.Key, header.Value);
                    }
                }
            }

            return result;
        }
    }
}