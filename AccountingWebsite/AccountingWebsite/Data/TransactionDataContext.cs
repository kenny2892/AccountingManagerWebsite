using AccountingWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingWebsite.Data
{
	public class TransactionDataContext : DbContext
	{
		public DbSet<Transaction> Transatcions { get; set; }

		public TransactionDataContext(DbContextOptions<TransactionDataContext> options) : base(options)
		{
		}
	}
}
