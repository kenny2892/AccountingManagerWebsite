using System;
using System.ComponentModel.DataAnnotations;

namespace AccountingWebsite.Models
{
    public class StatementMapping
    {
        [Key]
        public int Index { get; set; }
        public string Bank { get; set; }
        public string TransactionIdHeader { get; set; }
        public string PurchasingDateHeader { get; set; }
        public string PostingDateHeader { get; set; }
        public string AmountHeader { get; set; }
        public string CheckNumberHeader { get; set; }
        public string ReferenceNumberHeader { get; set; }
        public string DescriptionHeader { get; set; }
        public string CategoryOneHeader { get; set; }
        public string CategoryTwoHeader { get; set; }
        public string TypeHeader { get; set; }
        public string BalanceHeader { get; set; }
        public string MemoHeader { get; set; }
        public string ExtendedDescriptionHeader { get; set; }
        public string VendorNameHeader { get; set; }
        public string IsPurchaseHeader { get; set; }
        public string IsCreditHeader { get; set; }
        public string IsCheckHeader { get; set; }
        public string IsCreditPaymentHeader { get; set; }
    }
}
