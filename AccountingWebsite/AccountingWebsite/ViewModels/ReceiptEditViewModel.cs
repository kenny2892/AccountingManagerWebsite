using System;
using System.Collections.Generic;

namespace AccountingWebsite.ViewModels
{
    public class ReceiptEditViewModel
    {
        public int DisplayIndex { get; set; }
        public string FileName { get; set; }
        public string Vendor { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double Amount { get; set; }
        public List<string> VendorOptions { get; set; }
    }
}
