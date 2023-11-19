using AccountingWebsite.Data;
using AccountingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using AccountingWebsite.ViewModels;

namespace AccountingWebsite.Controllers
{
    public class ReportsController : Controller
    {
        private readonly TransactionDataContext _transactionContext;
        private readonly VendorContext _vendorContext;

        public ReportsController(TransactionDataContext transactionContext, VendorContext vendorContext)
        {
            _transactionContext = transactionContext;
            _vendorContext = vendorContext;
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionContext.Transactions.ToListAsync();
            var vendors = await _vendorContext.Vendors.ToListAsync();
            var categoryOptions = vendors.Select(vendor => (vendor.CategoryOne, vendor.CategoryTwo))
                .GroupBy(pair => pair.CategoryOne)
                .ToDictionary(group => group.Key, group => group
                    .Select(pair => pair.CategoryTwo).Distinct().Where(catTwo => !String.IsNullOrEmpty(catTwo)).OrderBy(catTwo => catTwo).ToArray());

            List<string> bankParameters = new List<string>();
            List<string> bankValues = new List<string>();
            var uniqueBanks = transactions.Select(trans => trans.Bank).Distinct().Order().ToList();
            for(int i = 0; i < uniqueBanks.Count; i++)
            {
                bankParameters.Add($"vm.Banks[{i}]");
                bankValues.Add(uniqueBanks[i]);
            }

            DropdownCheckboxViewModel bankVm = new DropdownCheckboxViewModel();
            bankVm.FormNames = bankParameters;
            bankVm.Values = bankValues;
            bankVm.Prompt = "Select Bank";

            List<string> vendorParameters = new List<string>();
            List<string> vendorValues = new List<string>();
            var uniqueVendors = vendors.Select(vendor => vendor.Name).Distinct().Order().ToList();
            for(int i = 0; i < uniqueVendors.Count; i++)
            {
                vendorParameters.Add($"vm.Vendors[{i}]");
                vendorValues.Add(uniqueVendors[i]);
            }

            DropdownCheckboxViewModel vendorVm = new DropdownCheckboxViewModel();
            vendorVm.FormNames = vendorParameters;
            vendorVm.Values = vendorValues;
            vendorVm.Prompt = "Select Vendors";

            return View(new ReportsViewModel(categoryOptions, bankVm, vendorVm));
        }

        // Loading the graphs
        public async Task<IActionResult> LoadGraphs(ReportCriteriaViewModel vm)
        {
            var transactions = await FilterTransactions(vm);
            return PartialView("_ReportDisplay", transactions);
        }

        // Loading the table
        public async Task<IActionResult> LoadTable(ReportCriteriaViewModel vm)
        {
            var transactions = await FilterTransactions(vm);

            if(vm.TableVm is null)
            {
                vm.TableVm = new TransactionTableViewModel();
            }

            vm.TableVm.ViewModelName = "vm.TableVm";
            vm.TableVm.InfiniteScroll = false;
            transactions = SortTransactions(transactions.AsQueryable(), vm.TableVm).ToList();
            transactions = FilterTransactions(transactions.AsQueryable(), vm.TableVm).ToList();
            vm.TableVm.Transactions = transactions;
            vm.TableVm.CurrentRowCount = vm.TableVm.Transactions.Count;

            return PartialView("../Transactions/_TransactionTable", vm.TableVm);
        }

        private async Task<List<Transaction>> FilterTransactions(ReportCriteriaViewModel vm)
        {
            var transactions = await _transactionContext.Transactions.ToListAsync();
            var receipts = await _transactionContext.Receipts.ToListAsync();

            int diff = 0;
            var today = DateTime.Now;
            if(vm.Timeframe != vm.DefaultOption)
            {
                switch(vm.Timeframe)
                {
                    case "This Week":
                        // Source: https://stackoverflow.com/a/38064
                        vm.EndDate = today;
                        diff = (7 + (int) today.DayOfWeek) % 7;
                        vm.StartDate = today.AddDays(-1 * diff).Date;
                        break;

                    case "Last Week":
                        diff = ((7 + (int) today.DayOfWeek) % 7) - 7;
                        vm.StartDate = today.AddDays(-1 * diff).Date;
                        vm.EndDate = today.AddDays(-1 * (diff + 6)).Date;
                        break;

                    case "This Month":
                        vm.StartDate = new DateTime(today.Year, today.Month, 1);
                        vm.EndDate = today;
                        break;

                    case "Last Month":
                        vm.StartDate = new DateTime(today.Year, today.Month - 1, 1);
                        vm.EndDate = new DateTime(today.Year, today.Month, 1).AddDays(-1);
                        break;

                    case "This Year":
                        vm.StartDate = new DateTime(today.Year, 1, 1);
                        vm.EndDate = today;
                        break;

                    case "Last Year":
                        vm.StartDate = new DateTime(today.Year - 1, 1, 1);
                        vm.EndDate = new DateTime(today.Year, 1, 1).AddDays(-1);
                        break;
                }

                transactions = transactions.Where(trans =>
                {
                    var dateToUse = trans.PurchasingDate;
                    var receiptMapping = receipts.FirstOrDefault(receipt => receipt.TransactionID == trans.TransactionID);

                    if(receiptMapping != null)
                    {
                        dateToUse = receiptMapping.PurchaseDate;
                    }

                    return vm.StartDate <= dateToUse && dateToUse <= vm.EndDate;
                }).ToList();
            }

            if(vm.CategoryOne != vm.DefaultOption)
            {
                transactions = transactions.Where(trans => trans.CategoryOne == vm.CategoryOne).ToList();
            }

            if(vm.CategoryTwo != vm.DefaultOption)
            {
                transactions = transactions.Where(trans => trans.CategoryTwo == vm.CategoryTwo).ToList();
            }

            foreach(var bankPair in vm.Banks)
            {
                var bankName = bankPair.Key;
                var status = bankPair.Value;

                switch(status)
                {
                    case "on":
                        transactions = transactions.Where(trans => trans.Bank.ToLower() == bankName.ToLower()).ToList();
                        break;

                    case "off":
                        transactions = transactions.Where(trans => trans.Bank.ToLower() != bankName.ToLower()).ToList();
                        break;
                }
            }

            switch(vm.IsPurchase)
            {
                case "on":
                    transactions = transactions.Where(trans => trans.IsPurchase).ToList();
                    break;

                case "off":
                    transactions = transactions.Where(trans => !trans.IsPurchase).ToList();
                    break;
            }

            switch(vm.IsCredit)
            {
                case "on":
                    transactions = transactions.Where(trans => trans.IsCredit).ToList();
                    break;

                case "off":
                    transactions = transactions.Where(trans => !trans.IsCredit).ToList();
                    break;
            }

            switch(vm.IsCheck)
            {
                case "on":
                    transactions = transactions.Where(trans => trans.IsCheck).ToList();
                    break;

                case "off":
                    transactions = transactions.Where(trans => !trans.IsCheck).ToList();
                    break;
            }

            switch(vm.IsCreditPayment)
            {
                case "on":
                    transactions = transactions.Where(trans => trans.IsCreditPayment).ToList();
                    break;

                case "off":
                    transactions = transactions.Where(trans => !trans.IsCreditPayment).ToList();
                    break;
            }

            foreach(var vendorPair in vm.Vendors)
            {
                var vendorName = vendorPair.Key;
                var status = vendorPair.Value;

                switch(status)
                {
                    case "on":
                        transactions = transactions.Where(trans => trans.VendorName.ToLower() == vendorName.ToLower()).ToList();
                        break;

                    case "off":
                        transactions = transactions.Where(trans => trans.VendorName.ToLower() != vendorName.ToLower()).ToList();
                        break;
                }
            }

            return transactions;
        }

        private IQueryable<Transaction> SortTransactions(IQueryable<Transaction> transactions, TransactionTableViewModel vm)
        {
            if(String.IsNullOrEmpty(vm.SortBy))
            {
                return transactions.OrderByDescending(transaction => transaction.PurchasingDate);
            }

            string propName = vm.SortBy.Replace("_", "");
            PropertyInfo propToSortBy = typeof(Transaction).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(propInfo => propInfo.Name.ToLower() == propName.ToLower()).FirstOrDefault();

            if(propToSortBy is null)
            {
                return transactions.OrderByDescending(transaction => transaction.PurchasingDate);
            }

            return vm.IsDescending ? transactions.OrderByDescending(propToSortBy) : transactions.OrderBy(propToSortBy);
        }

        private IQueryable<Transaction> FilterTransactions(IQueryable<Transaction> transactions, TransactionTableViewModel vm)
        {
            if(!String.IsNullOrEmpty(vm.BankSearch))
            {
                transactions = transactions.Where(transaction => transaction.Bank.ToLower().Contains(vm.BankSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.PurchasingDateSearch))
            {
                transactions = transactions.Where(transaction => transaction.PurchasingDate.ToString("G").ToLower().Contains(vm.PurchasingDateSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.PostingDateSearch))
            {
                transactions = transactions.Where(transaction => transaction.PostingDate.ToString("G").ToLower().Contains(vm.PostingDateSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.AmountSearch))
            {
                transactions = transactions.Where(transaction => transaction.AmountDisplay.ToLower().Contains(vm.AmountSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.DescriptionSearch))
            {
                transactions = transactions.Where(transaction => transaction.Description.ToLower().Contains(vm.DescriptionSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.CategoryOneSearch))
            {
                transactions = transactions.Where(transaction => transaction.CategoryOne.ToLower().Contains(vm.CategoryOneSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.CategoryTwoSearch))
            {
                transactions = transactions.Where(transaction => transaction.CategoryTwo.ToLower().Contains(vm.CategoryTwoSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.TypeSearch))
            {
                transactions = transactions.Where(transaction => transaction.Type.ToLower().Contains(vm.TypeSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.VendorNameSearch))
            {
                transactions = transactions.Where(transaction => transaction.VendorName.ToLower().Contains(vm.VendorNameSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.IsPurchaseSearch))
            {
                bool isEnabled = "true".StartsWith(vm.IsPurchaseSearch.ToLower()) || "yes".StartsWith(vm.IsPurchaseSearch.ToLower()) || "on".StartsWith(vm.IsPurchaseSearch.ToLower());
                transactions = transactions.Where(transaction => transaction.IsPurchase == isEnabled);
            }

            if(!String.IsNullOrEmpty(vm.IsCreditSearch))
            {
                bool isEnabled = "true".StartsWith(vm.IsCreditSearch.ToLower()) || "yes".StartsWith(vm.IsCreditSearch.ToLower()) || "on".StartsWith(vm.IsCreditSearch.ToLower());
                transactions = transactions.Where(transaction => transaction.IsCredit == isEnabled);
            }

            return transactions;
        }

        public async Task<IActionResult> ChartTesting()
        {
            var trans = await _transactionContext.Transactions.ToListAsync();
            var fastFoodTrans = trans.Where(trans => trans.CategoryTwo == "Fast Food");

            return View(fastFoodTrans.ToList());
        }
    }
}
