﻿using Bank2.Data;
using Bank2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Bank2.Controllers
{
  public class TransactionController : BaseController
  {
    public TransactionController(BankContext context) : base(context) { }
    public IActionResult BillingAndPayments()
    {
      return View();
    }

    public async Task<IActionResult> Transactions()
    {
      HttpContext.Session.SetString("LogStatus", "dash");
      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }
      var sharedInfo = await getSharedDataAsync(userId);
      return View(sharedInfo);
    }

    [Route("/Transaction/getTransaction")]
    public async Task<IActionResult> getTransaction(int accountId)
    {
      var userid = HttpContext.Session.GetInt32("UserId");
      var user = await _context.Users.Include(b => b.Branch).FirstOrDefaultAsync(u => u.Id == userid);
      var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
      var accounts = await _context.Accounts.Where(a => a.Id == accountId).ToListAsync();
      var transactions = await _context.Transactions
                        .Where(a => a.AccountId == accountId).OrderByDescending(x => x.Date)
                        .ToListAsync();

      var sharedInfo = new SharedData
      {
        User = user,
        SoloAccount = account,
        Account = account != null ? new List<Account> { account } : new List<Account>(),
        Transactions = transactions
      };
      return PartialView("partialTransaction", sharedInfo);
    }


    public async Task getAccountsAsync()
    {
      var userid = HttpContext.Session.GetInt32("UserId");
      if (userid.HasValue)
      {
        var account = await _context.Accounts.Where(x => x.UserId == userid.Value).ToListAsync();
        ViewData["Accounts"] = new SelectList(account, "Id", "AccountNo");
      }
    }
    public async Task<IActionResult> PaymentScreen()
    {
      await getAccountsAsync();
      return View();
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
          var receiver = await _context.Accounts.FirstOrDefaultAsync(x => x.AccountNo.ToLower() == data.toAccount.ToLower());

          if (currentUser == null || receiver == null)
          {
            ModelState.AddModelError("toAccount", "Account Don't Exist");
          }

          if ((currentAccount != null && receiver != null) && currentAccount.AccountBalance < data.amount)
          {
            ModelState.AddModelError("amount", "Insufficient Balance");
          }

          if (data.amount <= 0 && receiver != null)
          {
            ModelState.AddModelError("amount", "Amount Must be more than 0");
          }
          
          if(data.amount <= currentAccount.AccountBalance && receiver != null)
          {
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
            ModelState.AddModelError("amount", "Payment Success.");
          }
          await getAccountsAsync();
          return View("PaymentScreen");
        }
        catch (Exception e)
        {
          await transaction.RollbackAsync();
          await getAccountsAsync();
          return View("PaymentScreen");
        }
      }
    }
  }
}
