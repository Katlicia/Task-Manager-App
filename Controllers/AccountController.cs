using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApp.Entities;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                UserAccount account = new UserAccount();
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Email = model.Email;
                account.Password = model.Password;

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registired successfully. Please Login.";
                }
                catch (DbUpdateException ex)
                {

                    ModelState.AddModelError("", "Email is registired.");
                    return View(model);
                }
                
                return View();
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();   
                if (user != null)
                {
                    // Success, create cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FirstName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
                    return RedirectToAction("SecurePage");
                }
                else 
                {
                    ModelState.AddModelError("", "Email or Password is incorrect.");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var user = _context.UserAccounts.Find(userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (model.NewPassword == model.CurrentPassword)
                {
                    ModelState.AddModelError("", "Current password and new password can't be the same.");
                    return View(model);
                }

                if (user.Password != model.CurrentPassword)
                {
                    ModelState.AddModelError("", "Current password is incorrect.");
                    return View(model);
                }


                if (model.NewPassword != model.ConfirmedPassword)
                {
                    ModelState.AddModelError("", "The new password and confirmation password don't match.");
                    return View(model);
                }

                user.Password = model.NewPassword;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Password changed succesfully.";
                return RedirectToAction("SecurePage");

            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteAccountConfirmed()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _context.UserAccounts.Find(userId);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.UserAccounts.Remove(user);
                _context.SaveChanges();
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["SuccessMessage"] = "Your account has been deleted succesfully.";
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "An error occured while deleting this account.";
                return RedirectToAction("SecurePage");
            }
        }

    }
}
