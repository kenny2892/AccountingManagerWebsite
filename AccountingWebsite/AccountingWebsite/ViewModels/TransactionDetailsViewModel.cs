using AccountingWebsite.Models;
using System.Collections.Generic;

namespace AccountingWebsite.ViewModels
{
    public class TransactionDetailsViewModel
    {
        public Transaction Transaction { get; set; }
        public Receipt Receipt { get; set; }
        public List<TransactionItem> PossibleItems { get; set; }
        public List<Measurement> PossibleMeasurements { get; set; }
    }
}
