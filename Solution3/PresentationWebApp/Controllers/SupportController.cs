using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PresentationWebApp.Controllers
{
    public class SupportController : Controller
    {
        [HttpGet]
        public IActionResult Contact() //this will be used to load the page
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string email, string query) //this will be used to handle the form submission
        {

            //...inform the responsible staff

            /*
             * feedback = [Thank you for getting in touch with us. We will answer back asap]
             * warning = [Type in some question]
             * key = [value]
             * key = [value]
             */

            if (string.IsNullOrEmpty(query))
                ViewData["warning"] = "Type in some question";
            else
                ViewData["feedback"] = "Thank you for getting in touch with us. We will answer back asap";

            return View();
        }
    }
}
