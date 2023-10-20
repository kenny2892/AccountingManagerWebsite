using AccountingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingWebsite.Data
{
	public class DbInitializer
	{
		public static void Initialize(TransactionDataContext context)
		{
			if(context.Transatcions.Count() > 0)
			{
				return;
			}

			// Create random data
			var listOfCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var banks = new string[] { "Bank 1", "Bank 2", "Bank 3" };
			var transactionTypes = new string[] { "Credit", "Debit", "Check" };
			var categoryOnes = new string[] { "Cat 1", "Cat 2", "Cat 3" };
			var categoryTwos = new string[] { "Inner Cat 1", "Inner Cat 2", "Inner Cat 3" };
			var vendors = new string[] { "Vendor 1", "Vendor 2", "Vendor 3" };
			var minDate = new DateTime(2015, 1, 1);
			var dateRange = (DateTime.Today - minDate).Days;
			var rng = new Random();

			var transactions = new List<Transaction>();
			for(int i = 0; i < 150; i++)
			{
				var toAdd = new Transaction
				{
					TransactionID = new String(Enumerable.Repeat(listOfCharacters, 20).Select(characters => characters[rng.Next(characters.Length)]).ToArray()),
					Bank = banks[rng.Next(banks.Length)],
					PurchasingDate = minDate.AddDays(rng.Next(dateRange)),
					TransactionType = transactionTypes[rng.Next(transactionTypes.Length)],
					Amount = rng.NextDouble() * 200,
					Description = "Random Description " + rng.Next(50),
					CategoryOne = categoryOnes[rng.Next(categoryOnes.Length)],
					CategoryTwo = categoryTwos[rng.Next(categoryTwos.Length)],
					VendorName = vendors[rng.Next(vendors.Length)],
					IsPurchase = true
				};

				toAdd.PostingDate = toAdd.PurchasingDate;
				transactions.Add(toAdd);
			}

			context.Transatcions.AddRange(transactions);
			context.SaveChanges();
		}
	}
}
