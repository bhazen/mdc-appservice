using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using AppServiceMDC.MobileApp.DataObjects;
using AppServiceMDC.MobileApp.Models;
using Microsoft.Azure.Mobile.Server.Authentication;

namespace AppServiceMDC.MobileApp.Controllers
{
    //[Authorize]
    public class ExpenseController : TableController<Expense>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Expense>(context, Request);
        }

        // GET tables/Expense
        public IQueryable<Expense> GetAllExpense()
        {
            return Query(); 
        }

        // GET tables/Expense/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Expense> GetExpense(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Expense/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Expense> PatchExpense(string id, Delta<Expense> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Expense
        public async Task<IHttpActionResult> PostExpense(Expense item)
        {
            var actualAadUser = await User.GetAppServiceIdentityAsync<AzureActiveDirectoryCredentials>(Request);
            item.EmployeeName = actualAadUser.UserClaims.FirstOrDefault(c => c.Type == "name")?.Value ?? "N/A";
            Expense current = await InsertAsync(item);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Expense/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteExpense(string id)
        {
             return DeleteAsync(id);
        }
    }
}