using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AppServiceMDC.ApiApp.Utils;
using AppServiceMDC.ApiApp.ViewModels;
using Swashbuckle.Swagger.Annotations;

namespace AppServiceMDC.ApiApp.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [SwaggerOperation("GetAll")]
        public async Task<IEnumerable<UserViewModel>> Get()
        {
            var adClient = AuthenticationHelper.GetActiveDirectoryClient();
            var users = new List<UserViewModel>();
            var pagedCollection = await adClient.Users.ExecuteAsync();
            if (pagedCollection != null)
            {
                do
                {
                    foreach (var user in pagedCollection.CurrentPage.ToList())
                    {
                        users.Add(new UserViewModel { Name = user.DisplayName, Email = user.OtherMails.FirstOrDefault() });
                    }

                    pagedCollection = await pagedCollection.GetNextPageAsync();
                } while (pagedCollection != null);
            }

            return users;
        }

        [Route("{userId}")]
        [SwaggerOperation("GetById")]
        public async Task<UserViewModel> Get(string userId)
        {
            var adClient = AuthenticationHelper.GetActiveDirectoryClient();

            var adUser = await adClient.Users.GetByObjectId(userId).ExecuteAsync();

            return new UserViewModel
            {
                Name = adUser.DisplayName,
                Email = adUser.OtherMails.FirstOrDefault()
            };
        }
    }
}