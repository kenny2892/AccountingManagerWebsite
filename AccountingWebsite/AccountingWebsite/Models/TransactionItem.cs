using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingWebsite.Models
{
    public class TransactionItem
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
