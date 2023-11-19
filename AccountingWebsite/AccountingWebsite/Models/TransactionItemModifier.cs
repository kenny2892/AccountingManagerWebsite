using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingWebsite.Models
{
    public class TransactionItemModifier
    {
        [Key]
        public int ID { get; set; }

        // Parent Transaction Entry
        [ForeignKey("TransactionEntry")]
        public int? TransactionEntryID { get; set; }
        public virtual TransactionEntry TransactionEntry { get; set; }

        // Possible Parent Modifier
        [ForeignKey("ParentModifier")]
        public int? ParentModifierID { get; set; }
        public virtual TransactionItemModifier ParentModifier { get; set; }

        // Child Modifiers
        public virtual List<TransactionItemModifier> ChildModifiers { get; set; } = new List<TransactionItemModifier>();

        // Details
        [ForeignKey("Modifier")]
        public int ModifierID { get; set; }
        public virtual TransactionItem Modifier { get; set; }
        public decimal Quantity { get; set; }
        [ForeignKey("Measurement")]
        public int MeasurementID { get; set; }
        public virtual Measurement Measurement { get; set; }
        public decimal Cost { get; set; }
        public string Note { get; set; }
    }
}
