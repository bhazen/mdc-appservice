using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AppServiceMDC.ApiApp.DAL;
using AppServiceMDC.ApiApp.Models;
using AppServiceMDC.ApiApp.ViewModels;

namespace AppServiceMDC.ApiApp.Controllers
{
    [RoutePrefix("api/expenses")]
    public class ExpensesController : ApiController
    {
        [Route("last-month")]
        public ExpensesForMonthViewModel GetAllForLastMonth()
        {
            using (var repo = new ExpensesRepository())
            {
                var expensesByEmployee = repo.GetExpenseForPastMonth();

                var expenses = (from expense in expensesByEmployee
                    group expense by expense.EmployeeName
                    into expenseGroup
                    select expenseGroup).Select(grouping => new EmployeeExpenseReport
                    {
                        EmployeeName = grouping.Key,
                        Expenses = grouping
                    });

                return new ExpensesForMonthViewModel { ExpenseReports = expenses};
            }
        }

        [Route("categories")]
        [HttpGet]
        public IEnumerable<ExpenseCategory> Categories()
        {
            using (var repo = new ExpensesRepository())
            {
                return repo.GetCategories();
            }
        }
    }
}