using Microsoft.AspNetCore.Mvc;
using Bank2.Models;
using Bank2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
namespace Bank2.Controllers
{
  public class UserController : BaseController
  {
  public UserController(BankContext context) : base(context) { }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Remove all session data
        return RedirectToAction("LoginPage");
    }

    //When Logged in open dashboard
    public async Task<IActionResult> Dashboard()
    {
      HttpContext.Session.SetString("LogStatus", "dash");
      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }
      var account = await _context.Accounts.Where(x => x.UserId == userId.Value).ToListAsync();
      ViewData["Accounts"] = new SelectList(account, "Id", "AccountNo");
      return View();
    }


    //Return to Learn More View
    public IActionResult LearnMore()
    {
      return View();
    }

    //Return to Login Page
    public IActionResult LoginPage()
    {
        return View();
    }

    //Return to Notifications Page
    public IActionResult Notifications()
    {
      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }
      return View();
    }

    //Return to Security and Privacy Page
    public IActionResult SecurityAndPrivacy()
    {
      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }
      return View();
    }

    //Return to Profile Page
    public async Task<IActionResult> Profile()
    {
      HttpContext.Session.SetString("LogStatus", "dash");

      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
      return View(user);
    }
        
    //Main Action to Check Login
    [Route("User/CheckLogin")]
    [HttpPost]
    public async Task<IActionResult> CheckLogin([Bind("Username, Password")] User user)
    {
      var foundUser = await _context.Users.SingleOrDefaultAsync(x =>
      x.Username.ToLower() == user.Username.ToLower() ||
      x.Email.ToLower() == user.Username.ToLower());

      user.Password = GenerateHash256(user.Password + foundUser.Salt);
      if (foundUser != null && foundUser.Password == user.Password)
      {
          HttpContext.Session.SetString("Username", foundUser.Username ?? "Unknown User");
          HttpContext.Session.SetInt32("UserId", foundUser.Id);
          return RedirectToAction("LoginPage");
      }
      TempData["Error"] = "error";
      return RedirectToAction("LoginPage");
    }

    //Retruns the User Creation Page
    public IActionResult CreateUser()
    {
      ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
      return View();
    }

    //Creates User Profile and adds them to database
    [HttpPost]
    public async Task<IActionResult> CreateUserAccount(User users)
    {
      if(users != null)
      {
        var isUsername = await _context.Users.AnyAsync(u => u.Username == users.Username);
        var isEmail = await _context.Users.AnyAsync(u => u.Email == users.Email);
        var isPhone = await _context.Users.AnyAsync(u => u.Phone == users.Phone);
        if (isUsername)
        {
          ModelState.AddModelError("Username", "Username Already Exist");
        }
        if (isEmail)
        {
          ModelState.AddModelError("Email", "Email Already Exist");
        }
        if (isPhone)
        {
          ModelState.AddModelError("Phone", "Phone Already Exist");
        }
        ViewData["Branches"] = new SelectList(_context.Branchs, "Id", "Name");
        return View("CreateUser", users);
      }
     
      if (ModelState.IsValid)
      {
        users.Salt = GenerateSalt();
        users.Password = GenerateHash256(users.Password + users.Salt);
        _context.Users.Add(users);
        await _context.SaveChangesAsync();
        await CreateNewAccountAsync(users.Id, 0, users.CustomerType);
        return RedirectToAction("LoginPage");
      }
      return View("CreateUser", users);
    }

    //Used to update user profile
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(int id, ProfileUpdate? users)
    {
      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }

      try
      {
        var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if(users.OldPassword != null && userdata.Password != users.OldPassword)
        {
          Debug.WriteLine("Error: No Data");
          return RedirectToAction("Profile");
        }

        userdata.Username = users.Username;
        userdata.FullName = users.FullName;
        if(users.Password != null)
        {
          userdata.Password = users.Password;
          userdata.OldPassword = users.OldPassword;
        }
        userdata.BusinessName = users.BusinessName;
        userdata.BusinessType = users.BusinessType;
        
        userdata.TaxId = users.TaxId;
        userdata.Email = users.Email;
        userdata.Phone = users.Phone;
        userdata.Job = users.Job;
        userdata.Pin = users.Pin;
        userdata.Address = users.Address;
        userdata.CustomerType = userdata.CustomerType;

        _context.Users.Update(userdata);
        await _context.SaveChangesAsync();
        return RedirectToAction("Profile");
        }
      catch(Exception e)
      {
        return View(e.Message);
      }
    }

    public async Task<IActionResult> DeleteProfile()
    {
      var userId = HttpContext.Session.GetInt32("UserId");
      if (userId == null)
      {
        return RedirectToAction("LoginPage");
      }
      var userdata = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
      if(userdata != null)
      {
        _context.Users.Remove(userdata);
        await _context.SaveChangesAsync();
        HttpContext.Session.Clear();
      }
      return RedirectToAction("LoginPage");
    }
  }
}
