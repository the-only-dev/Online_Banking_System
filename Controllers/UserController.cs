using Microsoft.AspNetCore.Mvc;
using Bank2.Models;
using Bank2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Bank2.Controllers
{
    public class UserController : Controller
    {
        private readonly BankContext _context;
        public UserController(BankContext context)
        {
            _context = context;
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Remove all session data
            return RedirectToAction("LoginPage");
        }

        //public IActionResult Home()
        //{
        //    HttpContext.Session.SetString("LogStatus", "log");
        //    return RedirectToAction("LoginPage");
        //}
        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult Notifications()
        {
            return View();
        }

        public IActionResult SecurityAndPrivacy()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        //Return user information based on user logged in using id
        public async Task<IActionResult> Dashboard()
        {
           var userId = HttpContext.Session.GetInt32("UserId");
           if (userId.HasValue)
           {
                var user = await _context.Users
                    .Include(a => a.Accounts).ThenInclude(t => t.Transactions)
                    .Include(b => b.Branch)
                    .FirstOrDefaultAsync(x => x.Id == userId.Value);
                HttpContext.Session.SetString("LogStatus", "dash");
                return View(user);
           }
            return RedirectToAction("LoginPage");
        }


        [Route("User/CheckLogin")]
        [HttpPost]
        public async Task<IActionResult> CheckLogin([Bind("Username, Password")] User user)
        {
            var confirm = await _context.Users.SingleOrDefaultAsync(x =>
            x.Username == user.Username && 
            x.Password == user.Password);

            if (confirm != null)
            {
                HttpContext.Session.SetString("Username", confirm.Username ?? "Unknown User");
                HttpContext.Session.SetInt32("UserId", confirm.Id);
                return RedirectToAction("LoginPage");
            }
            TempData["Error"] = "error";
            return RedirectToAction("LoginPage");
        }

        public async Task<IActionResult> CreateAccount([Bind()] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("LoginPage");
        }
        public IActionResult CreateUser()
        {
            ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([Bind("CustomerType, BranchId, Username, Password, FullName, Email, Phone, Address, Pin, Job, BusinessName, BusinessType, TaxId")] User users)
        {
            if (ModelState.IsValid)
            { 
                _context.Users.Add(users);
                await _context.SaveChangesAsync();

                var account = new Account();
                account.UserId = users.Id;
                account.AccountNo = "A/C-0000-" + users.Id;
                account.AccountBalance = 0;
                account.CreatedAt = DateTime.Now;
                account.AccountType = "Savings";

                 _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return RedirectToAction("LoginPage");
            }
            ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
            return View(users);
        }

        public async Task<IActionResult> EditAccount(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
               
            ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditAccount(int id, [Bind("Id, CustomerType, BranchId, Username, Password, FullName, Email, Phone, Address, Pin, Job, BusinessName, BusinessType, TaxId")] User users)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(users);
                await _context.SaveChangesAsync();
                return RedirectToAction("Transactions");
            }
            ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
            return View(users);
        }

        public async Task<IActionResult> DeleteAccount(int id)
        {
            var user = await _context.Users.Include(b => b.Branch).FirstOrDefaultAsync(x => x.Id == id);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Deleted(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Transactions");
        }
      
        public async Task<IActionResult> Transactions()
        {
            var user = await _context.Users.Include(s => s.Accounts)
                                           .Include(b => b.Branch).ToListAsync();
            return View(user);
        }
        
    }
}
