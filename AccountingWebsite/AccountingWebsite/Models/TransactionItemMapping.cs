using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingWebsite.Models
{
    public class TransactionItemMapping
    {
        [Key]
        public int ID { get; set; }
        public string VendorName { get; set; }
        public List<string> Keyphrases { get; set; }
        [ForeignKey("Item")]
        public int ItemID { get; set; }
        public virtual TransactionItem Item { get; set; }
    }
}
