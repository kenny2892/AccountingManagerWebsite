using System;
using System.Collections.Generic;

namespace AccountingWebsite.ViewModels
{
    public class ReportCriteriaViewModel
    {
        public string Timeframe { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CategoryOne { get; set; }
        public string CategoryTwo { get; set; }
        public Dictionary<string, string> Banks { get; set; }
        public Dictionary<string, string> Vendors { get; set; }
        public string IsPurchase { get; set; }
        public string IsCredit { get; set; }
        public string IsCheck { get; set; }
        public string IsCreditPayment { get; set; }
        public string DefaultOption { get; set; }
        public TransactionTableViewModel TableVm { get; set; }
    }
}
