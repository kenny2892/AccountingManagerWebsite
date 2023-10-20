using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountingWebsite.Data;
using AccountingWebsite.Models;
using System.Reflection;

namespace AccountingWebsite.Controllers
{
    public class TransactionsController : BaseController
    {
        private readonly TransactionDataContext _context;
        private readonly int _perPageCount = 50;

        public TransactionsController(TransactionDataContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public IActionResult Index()
        {
            return View();
        }

        // Get the transactions to be loaded in
        public async Task<IActionResult> LoadTransactions(int existingRowCount, string sortBy, string prevSortBy, string prevSortOrder,
            string bankSearch, string purchasingDateSearch, string postingDateSearch, string transactionTypeSearch, string amountSearch, 
            string descriptionSearch, string categoryOneSearch, string categoryTwoSearch, string typeSearch, string vendorNameSearch, string isPurchaseSearch)
        {
            var transactions = SortTransactions(_context.Transatcions, sortBy, prevSortBy, prevSortOrder);
            transactions = FilterTransactions(transactions, bankSearch, purchasingDateSearch, postingDateSearch, transactionTypeSearch, amountSearch,
                descriptionSearch, categoryOneSearch, categoryTwoSearch, typeSearch, vendorNameSearch, isPurchaseSearch);

            transactions = transactions.Take(existingRowCount + _perPageCount);
            return PartialView("_TransactionTable", await transactions.ToListAsync());
        }

        private IQueryable<Transaction> SortTransactions(DbSet<Transaction> transatcions, string sortBy, string prevSortBy, string prevSortOrder)
        {
            if(String.IsNullOrEmpty(sortBy))
            {
                return transatcions.OrderByDescending(transaction => transaction.PurchasingDate);
            }

            // Get Property to Sort By
            // If the SortBy is Default, then order it by the previous
            string propName = sortBy == "default" ? prevSortBy.Replace("_", "") : sortBy.Replace("_", "");
            PropertyInfo propToSortBy = typeof(Transaction).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(propInfo => propInfo.Name.ToLower() == propName.ToLower()).FirstOrDefault();

            if(propToSortBy is null)
            {
                return transatcions.OrderByDescending(transaction => transaction.PurchasingDate);
            }

            // Get Sort Order
            bool isDesc = prevSortOrder == "desc";

            // If the property to sort by has not changed, then toggle the sort order
            // If the SortBy is Default, then order it by the previous
            if(sortBy != "default")
            {
                isDesc = prevSortBy == sortBy ? !isDesc : true;
            }

            // Update the ViewData
            ViewData["SortBy"] = sortBy == "default" ? prevSortBy : sortBy;
            ViewData["SortOrder"] = isDesc ? "desc" : "asc";

            return isDesc ? transatcions.OrderByDescending(propToSortBy) : transatcions.OrderBy(propToSortBy);
        }

        private IQueryable<Transaction> FilterTransactions(IQueryable<Transaction> transactions, string bankSearch, string purchasingDateSearch, 
            string postingDateSearch, string transactionTypeSearch, string amountSearch, string descriptionSearch, string categoryOneSearch, 
            string categoryTwoSearch, string typeSearch, string vendorNameSearch, string isPurchaseSearch)
        {
            if(!String.IsNullOrEmpty(bankSearch))
            {
                transactions = transactions.Where(transaction => transaction.Bank.ToLower().Contains(bankSearch.ToLower()));
                ViewData["BankSearch"] = bankSearch;
            }

            if(!String.IsNullOrEmpty(purchasingDateSearch))
            {
                transactions = transactions.Where(transaction => transaction.PurchasingDate.ToString("G").ToLower().Contains(purchasingDateSearch.ToLower()));
                ViewData["PurchasingDateSearch"] = purchasingDateSearch;
            }

            if(!String.IsNullOrEmpty(postingDateSearch))
            {
                transactions = transactions.Where(transaction => transaction.PostingDate.ToString("G").ToLower().Contains(postingDateSearch.ToLower()));
                ViewData["PostingDateSearch"] = postingDateSearch;
            }

            if(!String.IsNullOrEmpty(transactionTypeSearch))
            {
                transactions = transactions.Where(transaction => transaction.TransactionType.ToLower().Contains(transactionTypeSearch.ToLower()));
                ViewData["TransactionTypeSearch"] = transactionTypeSearch;
            }

            if(!String.IsNullOrEmpty(amountSearch))
            {
                transactions = transactions.Where(transaction => transaction.AmountDisplay.ToLower().Contains(amountSearch.ToLower()));
                ViewData["AmountSearch"] = amountSearch;
            }

            if(!String.IsNullOrEmpty(descriptionSearch))
            {
                transactions = transactions.Where(transaction => transaction.Description.ToLower().Contains(descriptionSearch.ToLower()));
                ViewData["DescriptionSearch"] = descriptionSearch;
            }

            if(!String.IsNullOrEmpty(categoryOneSearch))
            {
                transactions = transactions.Where(transaction => transaction.CategoryOne.ToLower().Contains(categoryOneSearch.ToLower()));
                ViewData["CategoryOneSearch"] = categoryOneSearch;
            }

            if(!String.IsNullOrEmpty(categoryTwoSearch))
            {
                transactions = transactions.Where(transaction => transaction.CategoryTwo.ToLower().Contains(categoryTwoSearch.ToLower()));
                ViewData["CategoryTwoSearch"] = categoryTwoSearch;
            }

            if(!String.IsNullOrEmpty(typeSearch))
            {
                transactions = transactions.Where(transaction => transaction.Type.ToLower().Contains(typeSearch.ToLower()));
                ViewData["TypeSearch"] = typeSearch;
            }

            if(!String.IsNullOrEmpty(vendorNameSearch))
            {
                transactions = transactions.Where(transaction => transaction.VendorName.ToLower().Contains(vendorNameSearch.ToLower()));
                ViewData["VendorNameSearch"] = vendorNameSearch;
            }

            if(!String.IsNullOrEmpty(isPurchaseSearch))
            {
                bool isEnabled = "true".StartsWith(isPurchaseSearch.ToLower()) || "yes".StartsWith(isPurchaseSearch.ToLower()) || "on".StartsWith(isPurchaseSearch.ToLower());
                transactions = transactions.Where(transaction => transaction.IsPurchase == isEnabled);
                ViewData["IsPurchaseSearch"] = isPurchaseSearch;
            }

            return transactions;
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Transatcions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transatcions
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionID,Bank,PurchasingDate,PostingDate,TransactionType,Amount,CheckNumber,ReferenceNumber,Description,CategoryOne,CategoryTwo,Type,Balance,Memo,ExtendedDescription,VendorName,ReceiptRelativeFilePath,IsPurchase")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Transatcions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transatcions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TransactionID,Bank,PurchasingDate,PostingDate,TransactionType,Amount,CheckNumber,ReferenceNumber,Description,CategoryOne,CategoryTwo,Type,Balance,Memo,ExtendedDescription,VendorName,ReceiptRelativeFilePath,IsPurchase")] Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Transatcions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transatcions
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Transatcions == null)
            {
                return Problem("Entity set 'TransactionDataContext.Transatcions'  is null.");
            }
            var transaction = await _context.Transatcions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transatcions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
          return _context.Transatcions.Any(e => e.TransactionID == id);
        }
    }
}
