using System;
using AccountingWebsite.Models;

namespace AccountingWebsite.ViewModels
{
    public class ReceiptDetailsViewModel
    {
        public Transaction Transaction { get; set; }
        public DateTime ReceiptPurchaseDate { get; set; }
        public string FileName { get; set; }
    }
}
