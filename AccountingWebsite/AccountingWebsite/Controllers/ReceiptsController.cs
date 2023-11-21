using AccountingWebsite.Data;
using AccountingWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AccountingWebsite.ViewModels;

namespace AccountingWebsite.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly TransactionDataContext _transactionContext;
        private readonly VendorContext _vendorContext;
        private IHostEnvironment _hostingEnvironment;

        public ReceiptsController(TransactionDataContext transactionContext, VendorContext vendorContext, IHostEnvironment hostingEnvironment)
        {
            _transactionContext = transactionContext;
            _vendorContext = vendorContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            string receiptPath = Path.Combine(_hostingEnvironment.ContentRootPath, "receipt_images");
            var receiptFiles = Directory.GetFiles(receiptPath).Where(file => file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg") || file.ToLower().EndsWith(".png"))
                .Select(file => Path.GetFileName(file)).ToList();

            ReceiptIndexViewModel vm = new ReceiptIndexViewModel();
            vm.ReceiptFileNames = receiptFiles.ToList();
            return View(vm);
        }

        public async Task<IActionResult> FilterReceipts(string mappedStatus)
        {
            string receiptPath = Path.Combine(_hostingEnvironment.ContentRootPath, "receipt_images");
            var receiptFiles = Directory.GetFiles(receiptPath)
                .Where(file => file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg") || file.ToLower().EndsWith(".png"))
                .Select(file => Path.GetFileName(file));
            var receiptPairings = await _transactionContext.Receipts.ToListAsync();

            switch(mappedStatus)
            {
                case "Only Mapped":
                    receiptFiles = receiptFiles.Where(fileName => receiptPairings.Any(pairing => pairing.FileName == fileName));
                    break;

                case "Only Not Mapped":
                    receiptFiles = receiptFiles.Where(fileName => !receiptPairings.Any(pairing => pairing.FileName == fileName));
                    break;
            }

            ReceiptIndexViewModel vm = new ReceiptIndexViewModel();
            vm.ReceiptFileNames = receiptFiles.ToList();
            vm.MappedStatus = mappedStatus;

            return View("Index", vm);
        }

        [HttpPost]
        public IActionResult FindReceiptDetails(string fileName)
        {
            ReceiptDetailsViewModel vm = new ReceiptDetailsViewModel();
            var details = _transactionContext.Receipts.FirstOrDefault(reciept => reciept.FileName == fileName);
            if(details != null)
            {
                vm.Transaction = _transactionContext.Transactions.FirstOrDefault(trans => trans.TransactionID == details.TransactionID);
                vm.ReceiptPurchaseDate = details.PurchaseDate;
            }

            vm.FileName = fileName;

            return PartialView("_ReceiptDetails", vm);
        }

        public async Task<IActionResult> Edit(string fileName)
        {
            var receipts = await _transactionContext.Receipts.ToListAsync();
            var receiptToEdit = receipts.FirstOrDefault(existing => existing.FileName == fileName);

            List<ReceiptEditViewModel> vms = new List<ReceiptEditViewModel>();
            var vendors = await _vendorContext.Vendors.ToListAsync();
            var transactions = await _transactionContext.Transactions.ToListAsync();

            var vm = new ReceiptEditViewModel();
            if(receiptToEdit != null)
            {
                var matchingTransaction = transactions.FirstOrDefault(existing => existing.TransactionID == receiptToEdit.TransactionID);
                vm.Vendor = matchingTransaction.VendorName;
                vm.Amount = matchingTransaction.Amount;
                vm.PurchaseDate = receiptToEdit.PurchaseDate;
            }

            else
            {
                string receiptPath = Path.Combine(_hostingEnvironment.ContentRootPath, "receipt_images");
                string filePath = Path.Combine(receiptPath, fileName);
                vm = ReceiptParser.ParseDetails(vendors, filePath);
            }

            vm.DisplayIndex = 0;
            vm.FileName = fileName;
            vm.VendorOptions = vendors.Select(vendor => vendor.Name).Order().ToList();
            vms.Add(vm);

            return View("Edit", vms);
        }

        [HttpPost]
        public async Task<IActionResult> UploadReceipt(List<IFormFile> receiptFiles)
        {
            if(receiptFiles != null && receiptFiles.Count > 0)
            {
                string receiptPath = Path.Combine(_hostingEnvironment.ContentRootPath, "receipt_images");
                List<ReceiptEditViewModel> vms = new List<ReceiptEditViewModel>();

                int index = 0;
                foreach(var receiptFile in receiptFiles)
                {
                    if(receiptFile != null && receiptFile.Length > 0)
                    {
                        var generatedFilePath = "";
                        if(String.IsNullOrEmpty(generatedFilePath) || System.IO.File.Exists(generatedFilePath))
                        {
                            generatedFilePath = Path.Combine(receiptPath, Guid.NewGuid().ToString() + Path.GetExtension(receiptFile.FileName));
                        }

                        try
                        {
                            using(Stream fileStream = new FileStream(generatedFilePath, FileMode.Create))
                            {
                                await receiptFile.CopyToAsync(fileStream);
                            }

                            var vendors = await _vendorContext.Vendors.ToListAsync();
                            var vm = ReceiptParser.ParseDetails(vendors, generatedFilePath);
                            vm.VendorOptions = vendors.Select(vendor => vendor.Name).Order().ToList();
                            vm.DisplayIndex = index++;
                            vms.Add(vm);
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }

                return View("Edit", vms);
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FindMatches(ReceiptEditViewModel vm)
        {
            var transactions = await _transactionContext.Transactions.ToListAsync();
            var matches = FindMatchingTransactions(vm, transactions);

            return PartialView("_MatchingReceipts", matches);
        }

        private List<Transaction> FindMatchingTransactions(ReceiptEditViewModel vm, List<Transaction> transactions)
        {
            var matches = new List<Transaction>();
            matches.AddRange(transactions);

            if(!String.IsNullOrEmpty(vm.Vendor))
            {
                matches = matches.Where(trans => trans.VendorName.ToLower() == vm.Vendor.ToLower()).ToList();
            }

            if(vm.Amount != 0)
            {
                matches = matches.Where(trans => Math.Abs(trans.Amount) == vm.Amount).ToList();
            }

            if(vm.PurchaseDate > DateTime.Parse("1/1/2000"))
            {
                matches = matches.Where(trans => trans.PurchasingDate.AddDays(-5) <= vm.PurchaseDate && vm.PurchaseDate <= trans.PurchasingDate.AddDays(5)).ToList();
            }

            return matches.OrderByDescending(trans => trans.PurchasingDate).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AssignMatch(ReceiptEditResultViewModel[] vms)
        {
            if(ModelState.IsValid)
            {
                List<Receipt> receipts = vms.Where(vm => vm.TransactionID != "--SKIP--")
                    .Select(vm => new Receipt() { FileName = vm.FileName, TransactionID = vm.TransactionID, PurchaseDate = vm.PurchaseDate }).ToList();
                var toUpdate = receipts.Where(receipt => _transactionContext.Receipts.Any(existing => existing.TransactionID == receipt.TransactionID)).ToList();
                var toAdd = receipts.Where(receipt => toUpdate.All(updating => updating.TransactionID != receipt.TransactionID)).ToList();

                foreach(var receipt in toUpdate)
                {
                    _transactionContext.Receipts.Update(receipt);
                }

                foreach(var receipt in toAdd)
                {
                    _transactionContext.Receipts.Add(receipt);
                }

                await _transactionContext.SaveChangesAsync();
                string receiptPath = Path.Combine(_hostingEnvironment.ContentRootPath, "receipt_images");
                return View("Index", new ReceiptIndexViewModel() { ReceiptFileNames = Directory.GetFiles(receiptPath).Where(file => file.EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png")).Select(file => Path.GetFileName(file)).ToList() });
            }

            return new EmptyResult();
        }
    }
}