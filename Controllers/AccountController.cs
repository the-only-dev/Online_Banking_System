using Bank2.Data;
using Bank2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bank2.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(BankContext context) : base(context) { }
    
        //Used to delete accounts from account management page
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

        //Create new account from account management page
        [Route("/Account/NewAccount")]
        [HttpPost]
        public async Task<IActionResult> NewAccount(int id, [Bind("accountType, amount")] SharedData data)
        {
          try
          {
            var userId = HttpContext.Session.GetInt32("UserId");
            var accounts = await _context.Accounts.Where(x => x.UserId == userId.Value).ToListAsync();
            if (accounts.Count > 2)
            {
              return RedirectToAction("AccountManagement", "Account");
            }

            if (!userId.HasValue)
            {
              return RedirectToAction("LoginPage", "User");
            }
            await CreateNewAccountAsync(userId, data.amount, data.accountType);
            return RedirectToAction("AccountManagement", "Account");
          }
          catch(Exception e)
          {
            return View(e.Message);
          }
        }

        //sends data for account management page
        [Route("/Account/AccountManagement")]
        public async Task<IActionResult> AccountManagement(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            if (!userid.HasValue)
            {
                return RedirectToAction("LoginPage", "User");
            }
            var sharedInfo = await getSharedDataAsync(userid);
            return View(sharedInfo);
        }
    }
}
