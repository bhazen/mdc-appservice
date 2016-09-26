using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppServiceMDC.XamarinForms.Helpers;
using AppServiceMDC.XamarinForms.Interfaces;
using AppServiceMDC.XamarinForms.Models;
using AppServiceMDC.XamarinForms.Pages;
using AppServiceMDC.XamarinForms.Services;
using Microsoft.WindowsAzure.MobileServices;
using MvvmHelpers;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.ViewModels
{
    public class ExpensesViewModel : BaseViewModel
    {
        private AzureService _azureService;

        public bool UserLoggedIn
        {
            get
            {
                Task.Run(() => _azureService.Initialize()).Wait();
                return _azureService.MobileService.CurrentUser != null;
            }
        }

        public ExpensesViewModel()
        {
            Title = "Expenses";
            _azureService = new AzureService();
        }

        public ObservableRangeCollection<Expense> Expenses { get; } = new ObservableRangeCollection<Expense>();
        public ObservableRangeCollection<Grouping<string, Expense>> ExpensesGrouped { get; } = new ObservableRangeCollection<Grouping<string, Expense>>();

        private string _loadingMessage;
        public string LoadingMessage
        {
            get { return _loadingMessage; }
            set { SetProperty(ref _loadingMessage, value); }
        }

        private ICommand _loadExpensesCommand;
        public ICommand LoadExpensesCommand => _loadExpensesCommand ?? (_loadExpensesCommand = new Command(async () => await ExecuteLoadExpensesCommand()));

        private async Task ExecuteLoadExpensesCommand()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                if (!AppServiceMDC.XamarinForms.Helpers.Settings.IsLoggedIn)
                {
                    await _azureService.Initialize();
                    var user =
                        await DependencyService.Get<IAuthentication>().LoginAsync(_azureService.MobileService, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                    if (user == null)
                    {
                        return;
                    }
                }

                LoadingMessage = "Loading expenses...";
                IsBusy = true;
                var expenses = await _azureService.GetExpenses();
                Expenses.ReplaceRange(expenses);
                SortExpenses();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading expenses: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SortExpenses()
        {
            var groups = from expense in Expenses
                orderby expense.DateIncurred descending
                group expense by expense.DateIncurred
                into expenseGroup
                select new Grouping<string, Expense>($"{expenseGroup.Key} ({expenseGroup.Count()})", expenseGroup);

            ExpensesGrouped.ReplaceRange(groups);
        }

        private ICommand _addExpenseCommand;
        public ICommand AddExpenseCommand => _addExpenseCommand ?? (_addExpenseCommand = new Command(async () => await ExecuteAddExpenseCommand()));

        private async Task ExecuteAddExpenseCommand()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                if (!AppServiceMDC.XamarinForms.Helpers.Settings.IsLoggedIn)
                {
                    await _azureService.Initialize();
                    var user = await DependencyService.Get<IAuthentication>().LoginAsync(_azureService.MobileService, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                    if (user == null)
                    {
                        return;
                    }
                }

                await Application.Current.MainPage.Navigation.PushAsync(new AddExpense());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error going to add expense: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private ICommand _logOutCommand;
        public ICommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new Command(async () => await ExecuteLogOutCommand()));

        private async Task ExecuteLogOutCommand()
        {
            await _azureService.MobileService.LogoutAsync();
            var athHelper = DependencyService.Get<IAuthentication>();
            athHelper.ClearCookies();
            Application.Current.MainPage = new EntryPage();
        }
    }
}
