using Bank2.Data;
using Bank2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bank2.Controllers
{
  public class BaseController : Controller
  {
    protected BankContext _context;

    public BaseController(BankContext context)
    {
      _context = context;
    }
    protected async Task<SharedData> getSharedData(int? id)
    {
      var branch = await _context.Branchs.ToListAsync();
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
      var accounts = await _context.Accounts.Where(x => x.UserId == id).ToListAsync();
      var accountIds = accounts.Select(a => a.Id).ToList();
      var transactions = await _context.Transactions
              .Where(t => accountIds.Contains(t.AccountId))
              .OrderByDescending(t => t.Date)
              .ToListAsync();

      return new SharedData
      {
        Branch = branch,
        User = user,
        Account = accounts,
        Transactions = transactions
      };
    }
  }
}
