using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingWebsite.Models
{
    public class Measurement
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public decimal Amount { get; set; }
        [ForeignKey("InnerMeasurement")]
        public int? InnerMeasurementID { get; set; }
        public virtual Measurement InnerMeasurement { get; set; }
        public bool IsCase { get; set; }
        public bool IsContainer { get; set; }

        public Measurement()
        {
            if(InnerMeasurementID is null)
            {
                InnerMeasurementID = -1;
            }
        }

        public string DisplayName()
        {
            if(IsCase && InnerMeasurement != null)
            {
                return $"Case - {((int) Amount == Amount ? (int) Amount : Math.Round(Amount, 2))}/{InnerMeasurement.DisplayName()}";
            }

            else if(IsContainer && InnerMeasurement != null)
            {
                return $"{Name} ({((int) Amount == Amount ? (int) Amount : Math.Round(Amount, 2))} {InnerMeasurement.ShortName})";
            }

            return Name;
        }
    }
}
