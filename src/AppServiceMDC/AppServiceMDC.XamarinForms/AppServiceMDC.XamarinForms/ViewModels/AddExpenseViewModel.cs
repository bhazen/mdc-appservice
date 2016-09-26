using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppServiceMDC.XamarinForms.Helpers;
using AppServiceMDC.XamarinForms.Interfaces;
using AppServiceMDC.XamarinForms.Models;
using AppServiceMDC.XamarinForms.Services;
using Microsoft.WindowsAzure.MobileServices;
using MvvmHelpers;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.ViewModels
{
    public class AddExpenseViewModel : BaseViewModel
    {
        private readonly AzureService _azureService;

        public AddExpenseViewModel()
        {
            Title = "Add Expense";
            _azureService = new AzureService();
            Categories = Task.Run(() => _azureService.GetExpenseCategories()).Result;
        }

        public DateTime Date { get; set; }

        public int CategoryIndex { get; set; }

        public decimal Amount { get; set; }

        public IEnumerable<ExpenseCategory> Categories { get; set; }

        private ICommand _submitExpenseCommand;

        public ICommand SubmitExpenseCommand
            =>
                _submitExpenseCommand ??
                (_submitExpenseCommand = new Command(async () => await ExecuteSubmitExpenseCommand()));

        private async Task ExecuteSubmitExpenseCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            var category = Categories.ElementAt(CategoryIndex);
            var expense = new Expense
            {
                Amount = Amount,
                CategoryId = category.Id,
                DateIncurred = Date,
                SubmittedBy = AppServiceMDC.XamarinForms.Helpers.Settings.UserId
            };

            try
            {
                if (!AppServiceMDC.XamarinForms.Helpers.Settings.IsLoggedIn)
                {
                    await _azureService.Initialize();
                    var user =
                        await
                            DependencyService.Get<IAuthentication>()
                                .LoginAsync(_azureService.MobileService, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                    if (user == null)
                    {
                        return;
                    }
                }

                await _azureService.AddExpense(expense);

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (AggregateException ex)
            {
                Debug.WriteLine($"Error adding expense: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}