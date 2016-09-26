using System;
using Microsoft.Azure.Mobile.Server;

namespace AppServiceMDC.MobileApp.DataObjects
{
    public class Expense : EntityData
    {
        public DateTime DateIncurred { get; set; }

        public decimal Amount { get; set; }

        public string CategoryId { get; set; }

        public string SubmittedBy { get; set; }

        public string EmployeeName { get; set; }
    }
}