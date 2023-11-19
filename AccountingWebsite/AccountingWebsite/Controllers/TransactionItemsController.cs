using AccountingWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingWebsite.Controllers
{
    public class TransactionItemsController : Controller
    {
        private readonly TransactionDataContext _transactionContext;
        private readonly VendorContext _vendorContext;

        public TransactionItemsController(TransactionDataContext transactionContext, VendorContext vendorContext)
        {
            _transactionContext = transactionContext;
            _vendorContext = vendorContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            var transactions = await _transactionContext.Transactions.ToListAsync();
            var trans = transactions.Where(trans => trans.PurchasingDate.Month == 10 && trans.PurchasingDate.Year == 2023 && trans.PurchasingDate.Day == 20 && trans.VendorName == "Wendy's").First();
            var receipts = await _transactionContext.Receipts.ToListAsync();
            var receipt = receipts.First(rec => rec.TransactionID == trans.TransactionID);

            

            return View();
        }
    }
}
