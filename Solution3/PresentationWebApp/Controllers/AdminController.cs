using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PresentationWebApp.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        UserManager<IdentityUser> _userManager;
        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpGet]
        public IActionResult Allocate()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Allocate(string email, string role)
        {
            var user = new IdentityUser { UserName = email, Email = email };

            _userManager.AddToRoleAsync(user, role);

            return View();
        }
    }
}
