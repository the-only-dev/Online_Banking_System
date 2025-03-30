using Bank2.Data;
using Bank2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bank2.Controllers
{
    public class AccountController : Controller
    {

        private readonly BankContext _context;

        public AccountController(BankContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> DeleteAccount(int id)
        {
            var acc = await _context.Accounts.FirstOrDefaultAsync(t => t.Id == id);
            if(acc != null)
            {
                _context.Accounts.Remove(acc);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AccountManagement", "Account");
        }

        [Route("/Account/NewAccount")]
        [HttpPost]
        public async Task<IActionResult> NewAccount(int id, [Bind("accountType, amount")] SharedData data)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var accounts = await _context.Accounts.Where(x => x.UserId == userId.Value).ToListAsync();
            if (accounts.Count > 2)
            {
                ViewData["MaxAccount"] = "You Have Reached Max Account Limit. Contact Support for more informaton.";
                return RedirectToAction("AccountManagement", "Account");
            }

            if (!userId.HasValue)
            {
                return RedirectToAction("LoginPage", "User");
            }
            var account = new Account();
            account.UserId = userId.Value;
            account.AccountBalance = data.amount;
            account.AccountNo = "A/C-0000-" + userId;
            account.CreatedAt = DateTime.Now;
            account.AccountStatus = "Active";
            account.AccountType = data.accountType;

            
            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();
            return RedirectToAction("AccountManagement", "Account");
        }

        [Route("/Account/AccountManagement")]
        public async Task<IActionResult> AccountManagement(int id)
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

            //ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
            return View(sharedInfo);
        }
    }
}
