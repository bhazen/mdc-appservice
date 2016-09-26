using System;

namespace AppServiceMDC.XamarinForms.Models
{
    public class Expense
    {
        public string Id { get; set; }

        public DateTime DateIncurred { get; set; }

        public decimal Amount { get; set; }

        public string CategoryId { get; set; }

        public string SubmittedBy { get; set; }
    }
}