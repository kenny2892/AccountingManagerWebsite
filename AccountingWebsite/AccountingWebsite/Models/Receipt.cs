using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingWebsite.Models
{
    public class Receipt
    {
        [Key]
        public string FileName { get; set; }
        public string TransactionID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Text { get; set; }
    }
}
