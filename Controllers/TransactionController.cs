using Bank2.Data;
using Bank2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace Bank2.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BankContext _context;

        public TransactionController(BankContext context)
        {
            _context = context;
        }

        public IActionResult BillingAndPayments()
        {
            return View();
        }

        public async Task<IActionResult> Payment()
        {
            try
            {
                var userid = HttpContext.Session.GetInt32("UserId");
                if (!userid.HasValue)
                {
                    return RedirectToAction("LoginPage", "User");
                }
                var branch = await _context.Branchs.ToListAsync();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.Value);
                var accounts = await _context.Accounts.Where(x => x.UserId == userid.Value).ToListAsync();
                var accountIds = accounts.Select(a => a.Id).ToList();
                var transactions = await _context.Transactions
                        .Where(t => accountIds.Contains(t.AccountId))
                        .OrderByDescending(t => t.Date)
                        .ToListAsync();


                int accountCount = accounts.Count;

                var sharedInfo = new SharedData
                {
                    Branch = branch,
                    User = user,
                    Account = accounts,
                    Transactions = transactions
                };

                ViewData["Accounts"] = new SelectList(accounts, "Id", "AccountNo");
                return View(sharedInfo);
            }
            catch(Exception e)
            {
                return View(e.Message);
            }
        }

        [Route("/Transaction/Pay")]
        public async Task<IActionResult> Pay(int id, [Bind("fromAccount, toAccount, amount, description")] SharedData data)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userid = HttpContext.Session.GetInt32("UserId");

                    if (userid == null)
                        return BadRequest();

                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userid.Value);
                    var currentAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == data.fromAccount);
                    var receiver = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNo == data.toAccount);

                    if (currentAccount == null || receiver == null)
                        return NotFound("One or both accounts not found.");

                    if (currentAccount.AccountBalance < data.amount)
                        return BadRequest("Insufficient balance.");

                    //Senders Transaction
                    var paymentFrom = new Transactions();
                    paymentFrom.Date = DateTime.Now;
                    paymentFrom.AccountId = currentAccount.Id;
                    paymentFrom.AccountNumber = currentAccount.AccountNo;
                    paymentFrom.Type = "Credit";
                    paymentFrom.Amount = data.amount;
                    paymentFrom.Description = " --- No Description ---";
                    if (data.description != null)
                    {
                        paymentFrom.Description = data.description;
                    }
                    currentAccount.AccountBalance -= data.amount;

                    paymentFrom.Balance = currentAccount.AccountBalance;
                    _context.Transactions.Add(paymentFrom);

                    //Receiver Transaction
                    var paymentTo = new Transactions();
                    paymentTo.Date = DateTime.Now;
                    paymentTo.AccountId = receiver.Id;
                    paymentTo.AccountNumber = receiver.AccountNo;
                    paymentTo.Type = "Debit";
                    paymentTo.Amount = data.amount;
                    paymentTo.Description = paymentFrom.Description;
                    receiver.AccountBalance += data.amount;

                    paymentTo.Balance = receiver.AccountBalance;
                    _context.Transactions.Add(paymentTo);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return RedirectToAction("Payment", "Transaction");
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    return View(e.Message);
                }
            }
        }
    }
}
