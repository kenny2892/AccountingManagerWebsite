using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingWebsite.Models
{
    public class TransactionEntry
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Transaction")]
        public string TransactionID { get; set; }
        public virtual Transaction Transaction { get; set; }

        [ForeignKey("Item")]
        public int TransactionItemID { get; set; }
        public virtual TransactionItem Item { get; set; }
        public decimal Quantity { get; set; }
        [ForeignKey("Measurement")]
        public int MeasurementID { get; set; }
        public virtual Measurement Measurement { get; set; }
        public decimal Cost { get; set; }
        public string Note { get; set; }
        public virtual List<TransactionItemModifier> Modifiers { get; set; } = new List<TransactionItemModifier>();
    }
}
