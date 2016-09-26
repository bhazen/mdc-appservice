using System.Configuration;
using System.Web.Mvc;

namespace AppServiceMDC.WebApp.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Expense
        public ActionResult Index()
        {
            ViewBag.BaseUrl = ConfigurationManager.AppSettings["ExpenseServiceUrl"];

            return View();
        }
    }
}