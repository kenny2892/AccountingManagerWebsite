using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountingWebsite.Data;
using AccountingWebsite.Models;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using AccountingWebsite.ViewModels;
using System.Diagnostics.Metrics;

namespace AccountingWebsite.Controllers
{
    public class TransactionsController : BaseController
    {
        private readonly TransactionDataContext _transactionContext;
        private readonly VendorContext _vendorContext;
        private readonly StatementContext _statementContext;
        private readonly int _perPageCount = 50;

        public TransactionsController(TransactionDataContext transactionContext, VendorContext vendorContext, StatementContext statementContext)
        {
            _transactionContext = transactionContext;
            _vendorContext = vendorContext;
            _statementContext = statementContext;
        }

        // GET: Transactions
        public IActionResult Index()
        {
            return View();
        }

        // Get the transactions to be loaded in
        public async Task<IActionResult> LoadTransactions(TransactionTableViewModel vm = null)
        {
            if(vm is null)
            {
                vm = new TransactionTableViewModel();
            }

            var transactions = SortTransactions(_transactionContext.Transactions, vm);
            transactions = FilterTransactions(transactions, vm);

            transactions = transactions.Take(vm.CurrentRowCount + _perPageCount);
            vm.Transactions = await transactions.ToListAsync();
            vm.CurrentRowCount = vm.Transactions.Count;

            return PartialView("_TransactionTable", vm);
        }

        private IQueryable<Transaction> SortTransactions(DbSet<Transaction> transatcions, TransactionTableViewModel vm)
        {
            if(String.IsNullOrEmpty(vm.SortBy))
            {
                return transatcions.OrderByDescending(transaction => transaction.PurchasingDate);
            }

            string propName = vm.SortBy.Replace("_", "");
            PropertyInfo propToSortBy = typeof(Transaction).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(propInfo => propInfo.Name.ToLower() == propName.ToLower()).FirstOrDefault();

            if(propToSortBy is null)
            {
                return transatcions.OrderByDescending(transaction => transaction.PurchasingDate);
            }

            return vm.IsDescending ? transatcions.OrderByDescending(propToSortBy) : transatcions.OrderBy(propToSortBy);
        }

        private IQueryable<Transaction> FilterTransactions(IQueryable<Transaction> transactions, TransactionTableViewModel vm)
        {
            if(!String.IsNullOrEmpty(vm.BankSearch))
            {
                transactions = transactions.Where(transaction => transaction.Bank.ToLower().Contains(vm.BankSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.PurchasingDateSearch))
            {
                transactions = transactions.Where(transaction => transaction.PurchasingDate.ToString("G").ToLower().Contains(vm.PurchasingDateSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.PostingDateSearch))
            {
                transactions = transactions.Where(transaction => transaction.PostingDate.ToString("G").ToLower().Contains(vm.PostingDateSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.AmountSearch))
            {
                transactions = transactions.Where(transaction => transaction.AmountDisplay.ToLower().Contains(vm.AmountSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.DescriptionSearch))
            {
                transactions = transactions.Where(transaction => transaction.Description.ToLower().Contains(vm.DescriptionSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.CategoryOneSearch))
            {
                transactions = transactions.Where(transaction => transaction.CategoryOne.ToLower().Contains(vm.CategoryOneSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.CategoryTwoSearch))
            {
                transactions = transactions.Where(transaction => transaction.CategoryTwo.ToLower().Contains(vm.CategoryTwoSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.TypeSearch))
            {
                transactions = transactions.Where(transaction => transaction.Type.ToLower().Contains(vm.TypeSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.VendorNameSearch))
            {
                transactions = transactions.Where(transaction => transaction.VendorName.ToLower().Contains(vm.VendorNameSearch.ToLower()));
            }

            if(!String.IsNullOrEmpty(vm.IsPurchaseSearch))
            {
                bool isEnabled = "true".StartsWith(vm.IsPurchaseSearch.ToLower()) || "yes".StartsWith(vm.IsPurchaseSearch.ToLower()) || "on".StartsWith(vm.IsPurchaseSearch.ToLower());
                transactions = transactions.Where(transaction => transaction.IsPurchase == isEnabled);
            }

            if(!String.IsNullOrEmpty(vm.IsCreditSearch))
            {
                bool isEnabled = "true".StartsWith(vm.IsCreditSearch.ToLower()) || "yes".StartsWith(vm.IsCreditSearch.ToLower()) || "on".StartsWith(vm.IsCreditSearch.ToLower());
                transactions = transactions.Where(transaction => transaction.IsCredit == isEnabled);
            }

            return transactions;
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _transactionContext.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _transactionContext.Transactions
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            var receipts = await _transactionContext.Receipts.FirstOrDefaultAsync(receipt => receipt.TransactionID == transaction.TransactionID);

            TransactionDetailsViewModel vm = new TransactionDetailsViewModel();
            vm.Transaction = transaction;
            vm.Receipt = receipts;
            vm.PossibleItems = await _transactionContext.TransactionItems.ToListAsync();
            vm.PossibleMeasurements = await _transactionContext.Measurements.ToListAsync();

            return View(vm);
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
        public async Task<IActionResult> Create([Bind("TransactionID,Bank,PurchasingDate,PostingDate,Amount,CheckNumber,ReferenceNumber,Description,CategoryOne,CategoryTwo,Type,Balance,Memo,ExtendedDescription,VendorName,ReceiptRelativeFilePath,IsPurchase")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _transactionContext.Add(transaction);
                await _transactionContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Import
        public IActionResult Import()
        {
            return View(false);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, bool isCredit, string bank)
        {
            List<Transaction> transactions = new List<Transaction>();
            var existingTransactions = await _transactionContext.Transactions.ToListAsync();
            using(var stream = file.OpenReadStream())
            using(var reader = new StreamReader(stream))
            {
                string content = await reader.ReadToEndAsync();
                transactions = ParseTransactions(content, isCredit, bank);
                transactions = transactions.Where(trans => existingTransactions.All(existing => existing.TransactionID != trans.TransactionID)).ToList();
            }

            return PartialView("_ImportResults", transactions);
        }

        private List<Transaction> ParseTransactions(string content, bool isCredit, string bank)
        {
            var existingTransactions = _transactionContext.Transactions.ToList();
            var lines = content.Split("\n").Where(line => !String.IsNullOrEmpty(line.Replace(",", ""))).ToList();
            var regex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");
            var headers = regex.Matches(lines[0]).Select(match => match.Value.Trim()).ToList();

            var statementMaps = _statementContext.StatementMappings.ToList().Where(map => 
            {
                var bankMatch = map.Bank.ToLower() == bank.ToLower();

                if(bankMatch)
                {
                    var id = headers.Any(header => header == map.TransactionIdHeader) || String.IsNullOrEmpty(map.TransactionIdHeader);
                    var purchaseDate = headers.Any(header => header == map.PurchasingDateHeader) || String.IsNullOrEmpty(map.PurchasingDateHeader);
                    var postDate = headers.Any(header => header == map.PostingDateHeader) || String.IsNullOrEmpty(map.PostingDateHeader);
                    var amount = headers.Any(header => header == map.AmountHeader) || String.IsNullOrEmpty(map.AmountHeader);
                    var check = headers.Any(header => header == map.CheckNumberHeader) || String.IsNullOrEmpty(map.CheckNumberHeader);
                    var refNum = headers.Any(header => header == map.ReferenceNumberHeader) || String.IsNullOrEmpty(map.ReferenceNumberHeader);
                    var desc = headers.Any(header => header == map.DescriptionHeader) || String.IsNullOrEmpty(map.DescriptionHeader);
                    var catOne = headers.Any(header => header == map.CategoryOneHeader) || String.IsNullOrEmpty(map.CategoryOneHeader);
                    var catTwo = headers.Any(header => header == map.CategoryTwoHeader) || String.IsNullOrEmpty(map.CategoryTwoHeader);
                    var type = headers.Any(header => header == map.TypeHeader) || String.IsNullOrEmpty(map.TypeHeader);
                    var balance = headers.Any(header => header == map.BalanceHeader) || String.IsNullOrEmpty(map.BalanceHeader);
                    var memo = headers.Any(header => header == map.MemoHeader) || String.IsNullOrEmpty(map.MemoHeader);
                    var extendDesc = headers.Any(header => header == map.ExtendedDescriptionHeader) || String.IsNullOrEmpty(map.ExtendedDescriptionHeader);
                    var vendor = headers.Any(header => header == map.VendorNameHeader) || String.IsNullOrEmpty(map.VendorNameHeader);
                    var isPurchase = headers.Any(header => header == map.IsPurchaseHeader) || String.IsNullOrEmpty(map.IsPurchaseHeader);
                    var isCredit = headers.Any(header => header == map.IsCreditHeader) || String.IsNullOrEmpty(map.IsCreditHeader);
                    var isCheck = headers.Any(header => header == map.IsCheckHeader) || String.IsNullOrEmpty(map.IsCheckHeader);
                    var isPayment = headers.Any(header => header == map.IsCreditPaymentHeader) || String.IsNullOrEmpty(map.IsCreditPaymentHeader);

                    return id && purchaseDate && postDate && amount && check && refNum && desc && catOne && catTwo && type && 
                        balance && memo && extendDesc && vendor && isPurchase && isCredit && isCheck && isPayment;
                }

                return false;
            }).ToList();

            if(statementMaps.Count == 0)
            {
                return new List<Transaction>();
            }

            var statementMap = statementMaps[0];
            var vendors = _vendorContext.Vendors.ToList();
            var entries = new List<Transaction>();

            foreach(string line in lines.Skip(1))
            {
                if(String.IsNullOrEmpty(line.Replace(",", "")))
                {
                    continue;
                }

                var cellValues = regex.Matches(line).Select(match => match.Value).ToList();
                var entry = new Transaction();
                entry.PurchasingDate = String.IsNullOrEmpty(statementMap.PurchasingDateHeader) ? DateTime.Parse("1/1/1990") : DateTime.Parse(cellValues[headers.IndexOf(statementMap.PurchasingDateHeader)]);
                entry.PostingDate = String.IsNullOrEmpty(statementMap.PostingDateHeader) ? DateTime.Parse("1/1/1990") : DateTime.Parse(cellValues[headers.IndexOf(statementMap.PostingDateHeader)]);
                entry.Amount = String.IsNullOrEmpty(statementMap.AmountHeader) ? 0: double.Parse(cellValues[headers.IndexOf(statementMap.AmountHeader)]);
                entry.CheckNumber = String.IsNullOrEmpty(statementMap.CheckNumberHeader) ? "" : cellValues[headers.IndexOf(statementMap.CheckNumberHeader)];
                entry.ReferenceNumber = String.IsNullOrEmpty(statementMap.ReferenceNumberHeader) ? "" : cellValues[headers.IndexOf(statementMap.ReferenceNumberHeader)];
                entry.Description = String.IsNullOrEmpty(statementMap.DescriptionHeader) ? "" : cellValues[headers.IndexOf(statementMap.DescriptionHeader)];
                entry.CategoryOne = String.IsNullOrEmpty(statementMap.CategoryOneHeader) ? "" : cellValues[headers.IndexOf(statementMap.CategoryOneHeader)];
                entry.CategoryTwo = String.IsNullOrEmpty(statementMap.CategoryTwoHeader) ? "" : cellValues[headers.IndexOf(statementMap.CategoryTwoHeader)];
                entry.Type = String.IsNullOrEmpty(statementMap.TypeHeader) ? "" : cellValues[headers.IndexOf(statementMap.TypeHeader)];
                entry.Balance = String.IsNullOrEmpty(statementMap.BalanceHeader) ? 0 : double.Parse(cellValues[headers.IndexOf(statementMap.BalanceHeader)]);
                entry.Memo = String.IsNullOrEmpty(statementMap.MemoHeader) ? "" : cellValues[headers.IndexOf(statementMap.MemoHeader)];
                entry.ExtendedDescription = String.IsNullOrEmpty(statementMap.ExtendedDescriptionHeader) ? "" : cellValues[headers.IndexOf(statementMap.ExtendedDescriptionHeader)];

                if(String.IsNullOrEmpty(statementMap.TransactionIdHeader))
                {
                    string properties = bank + entry.PurchasingDate.ToString("M-d-yyyy") + entry.AmountDisplay + entry.Description;

                    // Create a hash using the properties
                    using(SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(properties));

                        // Convert the hash bytes to a hexadecimal string
                        StringBuilder builder = new StringBuilder();
                        for(int i = 0; i < hashBytes.Length; i++)
                        {
                            builder.Append(hashBytes[i].ToString("x2"));
                        }

                        entry.TransactionID = builder.ToString();

                        while(existingTransactions.Any(existing => existing.TransactionID == entry.TransactionID) || entries.Any(existing => existing.TransactionID == entry.TransactionID))
                        {
                            entry.TransactionID += "-";
                        }
                    }
                }

                else
                {
                    entry.TransactionID = cellValues[headers.IndexOf(statementMap.TransactionIdHeader)];
                }

                var vendor = vendors.FirstOrDefault(vendor => vendor.TransactionKeyphrases.Any(keyphrase => entry.Description.ToLower().Contains(keyphrase.ToLower())));
                if(vendor != null)
                {
                    entry.VendorName = vendor.Name;
                    entry.CategoryOne = vendor.CategoryOne;
                    entry.CategoryTwo = vendor.CategoryTwo;
                }

                entry.Bank = bank;
                entry.IsCredit = isCredit;
                entry.IsCheck = !String.IsNullOrEmpty(entry.CheckNumber);
                entry.IsPurchase = (entry.IsCredit && entry.Amount > 0) || (!entry.IsCredit && entry.Amount < 0);
                entry.IsCreditPayment = false;

                if(entry.CategoryOne == "Bank" && entry.Description.ToLower().Contains("payment"))
                {
                    entry.IsPurchase = false;
                    entry.IsCreditPayment = true;
                }

                else if(entry.CategoryOne == "Bank" && entry.Description.ToLower().Contains("interest"))
                {
                    entry.IsPurchase = false;
                    entry.CategoryTwo = "Interest";
                }

                entries.Add(entry);
            }

            return entries;
        }

        private Dictionary<int, PropertyInfo> ParseHeaders(Type classType, List<string> lines, Regex regex)
        {
            var headers = regex.Matches(lines[0]).Select(match =>
            {
                var result = match.Value.Trim();

                // My bank's export uses "Posting Date" as the purchase date and "Effective Date" as the posting date. So this is to counteract that
                if(lines[0].Contains("Effective Date"))
                {
                    result = result.Replace("Posting Date", "Purchasing Date").Replace("Effective Date", "Posting Date");
                }

                if(result.StartsWith("\"") && result.EndsWith("\""))
                {
                    result = result[1..^1].Replace("\"\"", "\"");
                }

                return result;
            }).ToList();


            Dictionary<int, PropertyInfo> map = new Dictionary<int, PropertyInfo>();
            PropertyInfo[] propertyInfos = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(var propertyInfo in propertyInfos)
            {
                string match = headers.Where(header => header.Replace(" ", "").ToLower() == propertyInfo.Name.ToLower()).FirstOrDefault();

                if(!string.IsNullOrEmpty(match))
                {
                    map.Add(headers.IndexOf(match), propertyInfo);
                }
            }

            return map;
        }

        [HttpPost]
        public async Task<IActionResult> ImportRows(Transaction[] transactions)
        {
            if(ModelState.IsValid)
            {
                var existingTransactions = _transactionContext.Transactions;
                var toAdd = transactions.Where(newTrans => existingTransactions.All(existing => existing.TransactionID != newTrans.TransactionID)).ToList();

                foreach(var trans in toAdd)
                {
                    _transactionContext.Transactions.Add(trans);
                }

                await _transactionContext.SaveChangesAsync();
                return View("Index");
            }

            return View("Import", true);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _transactionContext.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _transactionContext.Transactions.FindAsync(id);
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
                    _transactionContext.Update(transaction);
                    await _transactionContext.SaveChangesAsync();
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
            if (id == null || _transactionContext.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _transactionContext.Transactions
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
            if (_transactionContext.Transactions == null)
            {
                return Problem("Entity set 'TransactionDataContext.Transatcions'  is null.");
            }
            var transaction = await _transactionContext.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _transactionContext.Transactions.Remove(transaction);
            }
            
            await _transactionContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
          return _transactionContext.Transactions.Any(e => e.TransactionID == id);
        }

        public async Task<string[]> GetItemNames()
        {
            var items = await _transactionContext.TransactionItems.ToListAsync();
            return items.Select(item => item.Name).Order().ToArray();
        }

        public async Task<int[]> GetItemIds()
        {
            var items = await _transactionContext.TransactionItems.ToListAsync();
            return items.OrderBy(item => item.Name).Select(item => item.ID).ToArray();
        }

        public async Task<string[]> GetMeasureNames()
        {
            var measures = await _transactionContext.Measurements.ToListAsync();
            var normals = measures.Where(measure => !measure.IsCase && !measure.IsContainer).Select(measure => measure.DisplayName()).Order().ToList();
            var containers = measures.Where(measure => measure.IsContainer && !normals.Any(existing => existing == measure.DisplayName()))
                .Select(measure => measure.DisplayName()).Order().ToList();
            var cases = measures.Where(measure => measure.IsCase && !normals.Any(existing => existing == measure.DisplayName()) && !containers.Any(existing => existing == measure.DisplayName()))
                .Select(measure => measure.DisplayName()).Order().ToList();

            var results = new List<string>();
            results.AddRange(normals);
            results.AddRange(containers);
            results.AddRange(cases);

            return results.ToArray();
        }

        public async Task<int[]> GetMeasureIds()
        {
            var measures = await _transactionContext.Measurements.ToListAsync();
            var normals = measures.Where(measure => !measure.IsCase && !measure.IsContainer).OrderBy(measure => measure.DisplayName()).Select(measure => measure.ID).ToList();
            var containers = measures.Where(measure => measure.IsContainer && !normals.Any(existing => existing == measure.ID))
                .OrderBy(measure => measure.DisplayName())
                .Select(measure => measure.ID).ToList();
            var cases = measures.Where(measure => measure.IsCase && !normals.Any(existing => existing == measure.ID) && !containers.Any(existing => existing == measure.ID))
                .OrderBy(measure => measure.DisplayName())
                .Select(measure => measure.ID).ToList();

            var results = new List<int>();
            results.AddRange(normals);
            results.AddRange(containers);
            results.AddRange(cases);

            return results.ToArray();
        }

        public async Task<bool> UpdateTransactionItems(TransactionItemsViewModel vm)
        {
            var transaction = _transactionContext.Transactions.FirstOrDefault(trans => trans.TransactionID == vm.TransactionId);

            if(transaction is null)
            {
                return false;
            }

            var items = await _transactionContext.TransactionItems.ToListAsync();
            var measurements = await _transactionContext.Measurements.ToListAsync();

            var mostRecentEntryRowId = -1;
            var mostRecentParentRowId = -1;
            var mostRecentChildRowId = -1;

            List<TransactionEntry> entriesToDelete = new List<TransactionEntry>();
            List<TransactionItemModifier> modsToDelete = new List<TransactionItemModifier>();

            for(int i = 0; i < vm.EntryTypes.Count; i++)
            {
                var typeToUse = vm.EntryTypes[i];
                var toDelete = vm.ToDelete[i];
                var transactionEntryIdToUse = vm.TransactionEntryIds[i];
                var modifierIdToUse = vm.ModifierIds[i];
                var parentModifierIdToUse = vm.ParentModifierIds[i];
                var itemIdToUse = vm.ItemIds[i];
                var qtyToUse = vm.Quantities[i];
                var measureIdToUse = vm.MeasurementIds[i];
                var costToUse = vm.Costs[i];
                var noteToUse = vm.Notes[i];

                if(toDelete)
                {
                    if(typeToUse == "Item" && transactionEntryIdToUse > -1)
                    {
                        var entry = _transactionContext.TransactionEntries.First(entry => entry.ID == transactionEntryIdToUse);
                        entriesToDelete.Add(entry);
                    }

                    else if(modifierIdToUse > -1)
                    {
                        var mod = _transactionContext.TransactionItemModifiers.First(mod => mod.ID == modifierIdToUse);
                        modsToDelete.Add(mod);
                    }

                    continue;
                }

                switch(typeToUse)
                {
                    case "Item":
                        mostRecentEntryRowId = AddOrUpdateTransactionEntry(transaction, items, measurements, transactionEntryIdToUse, itemIdToUse, qtyToUse, measureIdToUse, costToUse, noteToUse);
                        break;

                    case "ParentMod":
                        if(transactionEntryIdToUse < 0)
                        {
                            transactionEntryIdToUse = mostRecentEntryRowId;
                        }

                        mostRecentParentRowId = AddOrUpdateParentModifier(items, measurements, transactionEntryIdToUse, modifierIdToUse, itemIdToUse, qtyToUse, measureIdToUse, costToUse, noteToUse);
                        break;

                    case "ChildMod":
                        if(parentModifierIdToUse < 0)
                        {
                            parentModifierIdToUse = mostRecentParentRowId;
                        }

                        mostRecentChildRowId = AddOrUpdateChildModifier(items, measurements, modifierIdToUse, parentModifierIdToUse, itemIdToUse, qtyToUse, measureIdToUse, costToUse, noteToUse);
                        break;
                }

                _transactionContext.SaveChanges();
            }

            // Reverse so that child mods go before parent mods
            modsToDelete.Reverse();

            foreach(var delete in modsToDelete)
            {
                _transactionContext.TransactionItemModifiers.Remove(delete);
            }

            foreach(var delete in entriesToDelete)
            {
                _transactionContext.TransactionEntries.Remove(delete);
            }

            _transactionContext.SaveChanges();
            return true;
        }

        private int AddOrUpdateTransactionEntry(Transaction transaction, List<TransactionItem> items, List<Measurement> measurements, int transactionEntryIdToUse, int itemIdToUse, decimal qtyToUse, int measureIdToUse, decimal costToUse, string noteToUse)
        {
            var existingTransactionEntries = _transactionContext.TransactionEntries.Where(entry => entry.TransactionID == transaction.TransactionID).ToList();

            TransactionEntry transactionEntry = new TransactionEntry();
            bool alreadyExists = existingTransactionEntries.Any(entry => entry.ID == transactionEntryIdToUse);
            bool wasChanged = false;
            if(alreadyExists)
            {
                transactionEntry = existingTransactionEntries.First(entry => entry.ID == transactionEntryIdToUse);
            }

            else
            {
                transactionEntry.Transaction = transaction;
                transactionEntry.ID = _transactionContext.TransactionEntries.Count() + 1;
            }

            if(transactionEntry.TransactionItemID != itemIdToUse)
            {
                wasChanged = true;
                transactionEntry.Item = items.First(item => item.ID == itemIdToUse);
            }

            if(transactionEntry.Quantity != qtyToUse)
            {
                wasChanged = true;
                transactionEntry.Quantity = qtyToUse;
            }

            if(transactionEntry.MeasurementID != measureIdToUse)
            {
                wasChanged = true;
                transactionEntry.Measurement = measurements.First(meas => meas.ID == measureIdToUse);
            }

            if(transactionEntry.Cost != costToUse)
            {
                wasChanged = true;
                transactionEntry.Cost = costToUse;
            }

            if(transactionEntry.Note != noteToUse)
            {
                wasChanged = true;
                transactionEntry.Note = noteToUse;
            }

            if(alreadyExists && wasChanged)
            {
                _transactionContext.TransactionEntries.Update(transactionEntry);
            }

            else if(!alreadyExists && wasChanged)
            {
                _transactionContext.TransactionEntries.Add(transactionEntry);
            }

            return transactionEntry.ID;
        }

        private int AddOrUpdateParentModifier(List<TransactionItem> items, List<Measurement> measurements, int transactionEntryIdToUse, int modifierIdToUse, int itemIdToUse, decimal qtyToUse, int measureIdToUse, decimal costToUse, string noteToUse)
        {
            var mods = _transactionContext.TransactionItemModifiers.ToList();

            TransactionItemModifier parentMod = new TransactionItemModifier();
            bool alreadyExists = mods.Any(existingMod => existingMod.ID == modifierIdToUse);
            bool wasChanged = false;
            if(alreadyExists)
            {
                parentMod = mods.First(existingMod => existingMod.ID == modifierIdToUse);
            }

            else
            {
                parentMod.TransactionEntry = _transactionContext.TransactionEntries.First(entry => entry.ID == transactionEntryIdToUse);
                parentMod.ID = _transactionContext.TransactionItemModifiers.Count() + 1;
            }

            if(parentMod.ModifierID != itemIdToUse)
            {
                wasChanged = true;
                parentMod.Modifier = items.First(item => item.ID == itemIdToUse);
            }

            if(parentMod.Quantity != qtyToUse)
            {
                wasChanged = true;
                parentMod.Quantity = qtyToUse;
            }

            if(parentMod.MeasurementID != measureIdToUse)
            {
                wasChanged = true;
                parentMod.Measurement = measurements.First(meas => meas.ID == measureIdToUse);
            }

            if(parentMod.Cost != costToUse)
            {
                wasChanged = true;
                parentMod.Cost = costToUse;
            }

            if(parentMod.Note != noteToUse)
            {
                wasChanged = true;
                parentMod.Note = noteToUse;
            }

            if(alreadyExists && wasChanged)
            {
                _transactionContext.TransactionItemModifiers.Update(parentMod);
            }

            else if(!alreadyExists && wasChanged)
            {
                _transactionContext.TransactionItemModifiers.Add(parentMod);
            }

            return parentMod.ID;
        }

        private int AddOrUpdateChildModifier(List<TransactionItem> items, List<Measurement> measurements, int modifierIdToUse, int parentModifierIdToUse, int itemIdToUse, decimal qtyToUse, int measureIdToUse, decimal costToUse, string noteToUse)
        {
            var mods = _transactionContext.TransactionItemModifiers.ToList();

            TransactionItemModifier childMod = new TransactionItemModifier();
            bool alreadyExists = mods.Any(existingMod => existingMod.ID == modifierIdToUse);
            bool wasChanged = false;
            if(alreadyExists)
            {
                childMod = mods.First(existingMod => existingMod.ID == modifierIdToUse);
            }

            else
            {
                childMod.ParentModifier = mods.First(mod => mod.ID == parentModifierIdToUse);
                childMod.ID = _transactionContext.TransactionItemModifiers.Count() + 1;
            }

            if(childMod.ModifierID != itemIdToUse)
            {
                wasChanged = true;
                childMod.Modifier = items.First(item => item.ID == itemIdToUse);
            }

            if(childMod.Quantity != qtyToUse)
            {
                wasChanged = true;
                childMod.Quantity = qtyToUse;
            }

            if(childMod.MeasurementID != measureIdToUse)
            {
                wasChanged = true;
                childMod.Measurement = measurements.First(meas => meas.ID == measureIdToUse);
            }

            if(childMod.Cost != costToUse)
            {
                wasChanged = true;
                childMod.Cost = costToUse;
            }

            if(childMod.Note != noteToUse)
            {
                wasChanged = true;
                childMod.Note = noteToUse;
            }

            if(alreadyExists && wasChanged)
            {
                _transactionContext.TransactionItemModifiers.Update(childMod);
            }

            else if(!alreadyExists && wasChanged)
            {
                _transactionContext.TransactionItemModifiers.Add(childMod);
            }

            return childMod.ID;
        }

        public async Task<bool> CreateTransactionItem(string name, string note)
        {
            if(_transactionContext.TransactionItems.Any(trans => trans.Name.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", "")))
            {
                return false;
            }

            _transactionContext.TransactionItems.Add(new TransactionItem() { Name = name, Note = note });
            await _transactionContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateMeasure(string name, string shortname, bool isCase, bool isContainer, decimal amount, int? innerMeasureId)
        {
            if(_transactionContext.Measurements.Any(existing => existing.Name == name && existing.Amount == amount && existing.IsCase == isCase && existing.IsContainer == isContainer))
            {
                return false;
            }

            var newMeasure = new Measurement() { Name = name, ShortName = shortname, IsCase = isCase, IsContainer = isContainer, Amount = amount };
            if(newMeasure.IsCase || newMeasure.IsContainer || innerMeasureId is null)
            {
                newMeasure.InnerMeasurementID = innerMeasureId;
            }

            _transactionContext.Measurements.Add(newMeasure);
            await _transactionContext.SaveChangesAsync();
            return true;
        }
    }
}
