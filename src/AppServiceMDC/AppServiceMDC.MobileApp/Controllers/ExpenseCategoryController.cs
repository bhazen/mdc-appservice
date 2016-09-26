using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using AppServiceMDC.MobileApp.DataObjects;
using AppServiceMDC.MobileApp.Models;

namespace AppServiceMDC.MobileApp.Controllers
{
    public class ExpenseCategoryController : TableController<ExpenseCategory>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            MobileServiceContext context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<ExpenseCategory>(context, Request);
        }

        // GET tables/ExpenseCategory
        public IQueryable<ExpenseCategory> GetAllExpenseCategory()
        {
            return Query(); 
        }

        // GET tables/ExpenseCategory/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ExpenseCategory> GetExpenseCategory(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ExpenseCategory/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ExpenseCategory> PatchExpenseCategory(string id, Delta<ExpenseCategory> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ExpenseCategory
        public async Task<IHttpActionResult> PostExpenseCategory(ExpenseCategory item)
        {
            ExpenseCategory current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ExpenseCategory/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteExpenseCategory(string id)
        {
             return DeleteAsync(id);
        }
    }
}
