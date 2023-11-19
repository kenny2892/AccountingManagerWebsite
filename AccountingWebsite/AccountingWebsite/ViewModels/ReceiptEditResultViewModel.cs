using System;

namespace AccountingWebsite.ViewModels
{
    public class ReceiptEditResultViewModel
    {
        public string FileName { get; set; }
        public string TransactionID { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
