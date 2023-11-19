using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingWebsite.Models
{
    public class Vendor
    {
        [Key]
        public string Name { get; set; }
        public List<string> TransactionKeyphrases { get; set; } = new List<string>();
        public List<string> ReceiptKeyphrases { get; set; } = new List<string>();
        public List<string> ReceiptTotalKeyphrases { get; set; } = new List<string>();
        public string CategoryOne { get; set; }
        public string CategoryTwo { get; set; }
    }
}
