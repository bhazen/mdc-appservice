using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AppServiceMDC.XamarinForms.Helpers;
using AppServiceMDC.XamarinForms.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace AppServiceMDC.XamarinForms.Services
{
    public class AzureService
    {
        public MobileServiceClient MobileService { get; set; }
        private IMobileServiceSyncTable<Expense> _expenseTable;
        private IMobileServiceSyncTable<ExpenseCategory> _expenseCategoryTable;

        private bool _initialized;

        public async Task Initialize()
        {
            if (_initialized)
            {
                return;
            }

            var handler = new AuthHandler();
            MobileService = new MobileServiceClient("", handler);
            MobileService.AlternateLoginHost = new Uri("");
            handler.Client = MobileService;

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                MobileService.CurrentUser = new MobileServiceUser(Settings.UserId);
                MobileService.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }

            const string path = "appServiceMdc.db";
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Expense>();
            store.DefineTable<ExpenseCategory>();

            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            _expenseTable = MobileService.GetSyncTable<Expense>();
            _expenseCategoryTable = MobileService.GetSyncTable<ExpenseCategory>();

            _initialized = true;
        }

        public async Task<IEnumerable<Expense>> GetExpenses()
        {
            await Initialize();
            await SyncExpenses();
            var userId = AppServiceMDC.XamarinForms.Helpers.Settings.UserId;

            return await _expenseTable.Where(e => e.SubmittedBy == userId).OrderBy(e => e.DateIncurred).ToEnumerableAsync();
        }

        public async Task<Expense> AddExpense(Expense expense)
        {
            await Initialize();

            await _expenseTable.InsertAsync(expense);

            return expense;
        }

        public async Task SyncExpenses()
        {
            try
            {
                await _expenseTable.PullAsync("userExpenses", _expenseTable.CreateQuery());
                await MobileService.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to sync expenses: {ex}");
            }
        }

        public async Task<IEnumerable<ExpenseCategory>> GetExpenseCategories()
        {
            await Initialize();
            await _expenseCategoryTable.PullAsync("expenseCategories", _expenseCategoryTable.CreateQuery());

            return await _expenseCategoryTable.ToEnumerableAsync();
        }
    }
}