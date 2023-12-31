﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingWebsite.Models
{
	public class Transaction
	{
		[Key]
		public string TransactionID { get; set; }
		public string Bank { get; set; }
		public DateTime PurchasingDate { get; set; }
		public DateTime PostingDate { get; set; }
		public double Amount { get; set; }
        public string AmountDisplay { get { return Amount.ToString("C2"); } }
		public string CheckNumber { get; set; }
		public string ReferenceNumber { get; set; }
		public string Description { get; set; }
		public string CategoryOne { get; set; }
		public string CategoryTwo { get; set; }
		public string Type { get; set; }
		public double Balance { get; set; }
        public string BalanceDisplay { get { return Balance.ToString("C2"); } }
        public string Memo { get; set; }
		public string ExtendedDescription { get; set; }
		public string VendorName { get; set; }
		public bool IsPurchase { get; set; }
		public bool IsCredit { get; set; }
		public bool IsCheck { get; set; }
		public bool IsCreditPayment { get; set; }
        public virtual List<TransactionEntry> TransactionEntries { get; set; } = new List<TransactionEntry>();
    }
}
