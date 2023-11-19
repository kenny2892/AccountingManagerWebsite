using System.Collections.Generic;

namespace AccountingWebsite.ViewModels
{
    public class TransactionItemsViewModel
    {
        public string TransactionId { get; set; }
        public List<string> EntryTypes { get; set; }
        public List<bool> ToDelete { get; set; }
        public List<int> TransactionEntryIds { get; set; }
        public List<int> ModifierIds { get; set; }
        public List<int> ParentModifierIds { get; set; }
        public List<int> ItemIds { get; set; }
        public List<decimal> Quantities { get; set; }
        public List<int> MeasurementIds { get; set; }
        public List<decimal> Costs { get; set; }
        public List<string> Notes { get; set; }
    }
}
