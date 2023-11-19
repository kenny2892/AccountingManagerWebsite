using System;
using System.Collections.Generic;
using AccountingWebsite.Models;

namespace AccountingWebsite.ViewModels
{
    public class TransactionTableViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public string SortBy { get; set; } = "purchasing_date";
        public bool IsDescending { get; set; } = true;
        public string BankSearch { get; set; } = "";
        public string PurchasingDateSearch { get; set; }
        public string PurchasingDateMode { get; set; } = "";
        public string PostingDateSearch { get; set; }
        public string AmountSearch { get; set; } = "";
        public string DescriptionSearch { get; set; } = "";
        public string CategoryOneSearch { get; set; } = "";
        public string CategoryTwoSearch { get; set; } = "";
        public string TypeSearch { get; set; } = "";
        public string VendorNameSearch { get; set; } = "";
        public string IsPurchaseSearch { get; set; } = "";
        public string IsCreditSearch { get; set; } = "";
        public bool InfiniteScroll { get; set; } = true;
        public int CurrentRowCount { get; set; } = 0;
        public string ViewModelName { get; set; } = "vm";
    }
}
