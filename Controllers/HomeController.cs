using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccount.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BankAccount.Controllers
{
    public class HomeController : Controller
    {

        private YourContext _context;//need the next 6 lines for YourContext to work with this controller

        public HomeController(YourContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register(User NewUser)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.password = Hasher.HashPassword(NewUser, NewUser.password);
                //Save your user object to the database
                _context.Add(NewUser);
                _context.SaveChanges();

                System.Console.WriteLine("********************Register success********************");
                Account NewAccount = new Account
                {
                    balance = 0,
                    transaction = 0,
                    transactionDate = DateTime.Now,
                    users_Id = NewUser.Id
                };
                _context.Add(NewAccount);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("user_id", NewUser.Id);
                System.Console.WriteLine("********************" + HttpContext.Session.GetInt32("user_id") + "********************");
                System.Console.WriteLine("********************New Account Success********************");
                return RedirectToAction("Account");//ad the "Registration" cshtml later
            }
            else
            {
                System.Console.WriteLine("********************Register failed********************");               
                return View("Index",NewUser);

            }
        }

        public IActionResult Login(User ExistingUser)
        {
            //Attempt to retrieve a user from your database based on the Email submitted
            var user = _context.users.SingleOrDefault(u => u.email == ExistingUser.email);
            //List<User> ReturnedUser = _context.users.Where(u => u.email == email && u.password == password).ToList();
            if(user != null &&  ExistingUser.password!= null)
            {
                var Hasher = new PasswordHasher<User>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(user, user.password, ExistingUser.password ))//PasswordToCheck
                {
                    //Handle success
                    HttpContext.Session.SetInt32("user_id", user.Id);
                    System.Console.WriteLine("********************" + HttpContext.Session.GetInt32("user_id") + "********************");
                    System.Console.WriteLine("********************Login success********************");
                    return RedirectToAction("Account");//will add this route later "Registration"
                }

                System.Console.WriteLine("********************Login failed for bad pw********************");
                return RedirectToAction("Index");
            }
            //Handle failure
            else
            {
                System.Console.WriteLine("********************Login failed for bad email and pw********************");
                return RedirectToAction("Index");
               
            }
        }

        public IActionResult Account()
        {
            List<User> CurrentUser = _context.users.Where(user => user.Id == HttpContext.Session.GetInt32("user_id")).ToList();
            ViewBag.User = CurrentUser;
            List<Account> UserAccounts = _context.accounts.Where(acct => acct.users_Id == HttpContext.Session.GetInt32("user_id")).OrderByDescending(charge => charge.transactionDate).ToList();
            ViewBag.Accounts = UserAccounts;

            return View();
        }

        public IActionResult Transaction(double transaction)
        {
            
            var user = _context.users.SingleOrDefault(u => u.Id == HttpContext.Session.GetInt32("user_id"));
            if (ModelState.IsValid)
            {
                var MostRecentAcct = _context.accounts.Where(acct => acct.users_Id == HttpContext.Session.GetInt32("user_id")).OrderByDescending(charge => charge.transactionDate).First();
                System.Console.WriteLine("********************" + MostRecentAcct.balance + "********************");
                if (MostRecentAcct.balance + transaction < 0)
                {
                    return RedirectToAction("Account");
                }
                else
                {
                    Account NewCharge = new Account
                    {
                        transaction = transaction,
                        transactionDate = DateTime.Now,
                        users_Id = user.Id,
                        balance = MostRecentAcct.balance + transaction
                    };
                    _context.Add(NewCharge);
                    _context.SaveChanges();
                }
                return RedirectToAction("Account");
            }
            else
            {
                return RedirectToAction("Account");
            }
        }

        public IActionResult Need2Register()
        {
            ViewData["Message"] = "Please Register for you account.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
