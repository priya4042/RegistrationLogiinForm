using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RegistrationLogiinForm.Data;
using RegistrationLogiinForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationLogiinForm.Controllers
{
    public class AccountController : Controller
    {
        private DataAccess dataAccess;

        public AccountController(IConfiguration configuration)
        {
            dataAccess = new DataAccess(configuration);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                bool isRegistered = dataAccess.RegisterUser(user);
                if (isRegistered)
                    return RedirectToAction("Login");
                else
                    ModelState.AddModelError("", "Error in registration");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                bool isValidUser = dataAccess.ValidateUser(user.Email, user.Password);
                if (isValidUser)
                {
                    // Store user information in session
                    HttpContext.Session.SetString("Email", user.Email);

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                    ModelState.AddModelError("", "Invalid email or password");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            // Remove user information from session on logout
            HttpContext.Session.Remove("Email");

            return RedirectToAction("Login");
        }
    }
}