using System.Collections.Generic;
using AppServiceMDC.ApiApp.Models;

namespace AppServiceMDC.ApiApp.ViewModels
{
    public class ExpensesForMonthViewModel
    {
        public IEnumerable<EmployeeExpenseReport> ExpenseReports { get; set; }
    }

    public class EmployeeExpenseReport
    {
        public string EmployeeName { get; set; }

        public IEnumerable<Expense> Expenses { get; set; }
    }
}