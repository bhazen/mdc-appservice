using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AppServiceMDC.ApiApp.Models;
using Dapper;

namespace AppServiceMDC.ApiApp.DAL
{
    public class ExpensesRepository : IDisposable
    {
        private readonly IDbConnection _dbConnection;

        public ExpensesRepository()
        {
            _dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Expenses"].ConnectionString);
        }

        public IEnumerable<Expense> GetExpenseForPastMonth()
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;
            var parameters = new
            {
                @StartDate = new DateTime(currentYear, currentMonth, 1).ToString("d"),
                @EndDate = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth)).ToString("d")
            };
            return _dbConnection.Query<Expense>(@"
                SELECT 
                    e.*,
                    ec.Name as Category
                FROM 
                    Expenses e
                INNER JOIN
                    ExpenseCategories ec ON e.CategoryId = ec.Id
                WHERE 
                    e.DateIncurred BETWEEN CONVERT(datetime, @StartDate) AND CONVERT(datetime, @EndDate)",
                parameters);
        }

        public IEnumerable<ExpenseCategory> GetCategories()
        {
            return _dbConnection.Query<ExpenseCategory>(@"SELECT Id, Name From ExpenseCategories");
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}