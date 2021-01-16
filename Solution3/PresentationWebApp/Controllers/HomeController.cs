using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentationWebApp.Models;
using ShoppingCart.Domain.Models;

namespace PresentationWebApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly UserManager<Member> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger/*, UserManager<Member> userManager*/)
        {
            _logger = logger;
            //_userManager = userManager;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Started Index Method");

                _logger.LogWarning("About to raise an exception");

            }catch(Exception ex)
            {
                _logger.LogError("Message Occured: " + ex.Message);
            }

           //ViewBag.userId = _userManager.GetUserId(HttpContext.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
