using System;

namespace AppServiceMDC.ApiApp.Models
{
    public class Expense
    {
        public string Id { get; set; }

        public DateTime DateIncurred { get; set; }

        public decimal Amount { get; set; }

        public string Category { get; set; }

        public string SubmittedBy { get; set; }

        public string EmployeeName { get; set; }
    }
}