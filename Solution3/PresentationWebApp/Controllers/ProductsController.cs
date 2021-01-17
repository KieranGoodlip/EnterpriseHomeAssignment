using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagedList;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using cloudscribe.Pagination.Models;
using Microsoft.Extensions.Logging;

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    { 
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICartsService _cartsService;
        private readonly IOrdersService _ordersService;
        private readonly ILogger<ProductsController> _productsLogger;

        private IWebHostEnvironment _env;

        //List<ProductViewModel> productsList = new List<ProductViewModel>();
        List<CartViewModel> cartsList = new List<CartViewModel>();

        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, ICartsService cartsService, IOrdersService ordersService, ILogger<ProductsController> logger,
             IWebHostEnvironment env )
        {
            _ordersService = ordersService;
            _cartsService = cartsService;
            _productsService = productsService;
            _categoriesService = categoriesService;
            _env = env;
            _productsLogger = logger;
        }

        public IActionResult Index(int pageNumber = 0)
        {
            var catList = _categoriesService.GetCategories();
            ViewBag.Categories = catList;

            int pageSize = 10;

            var list = _productsService.GetProducts();//.ToList();

            int totalElements = list.Count();

            var listInPage = list.Skip(pageNumber * pageSize).Take(pageSize);

            int noOfPages = totalElements / pageSize;

            ViewBag.totalPages = (noOfPages - (totalElements % pageSize == 0 ? 1 : 0));

            ViewBag.pageNumber = pageNumber;

            ViewBag.productsList = list;

            _productsLogger.LogInformation("Index Method loaded succesfully");

            return View(listInPage);
            
        }

        [HttpPost]
        public IActionResult Search(int category) //using a form, and the select list must have name attribute = category
        {
            var catList = _categoriesService.GetCategories();
            ViewBag.Categories = catList;

            var list = _productsService.GetProducts(category);

            _productsLogger.LogInformation("Searched By Category");

            return View("Index", list);
        }


        public IActionResult Details(Guid id)
        {
            var p = _productsService.GetProduct(id);
            return View(p);
        }

        //the engine will load a page with empty fields
        [HttpGet]
        [Authorize (Roles ="Admin")] //is going to be accessed only by authenticated users
        public IActionResult Create()
        {
            //fetch a list of categories
            var listOfCategeories = _categoriesService.GetCategories();

            //we pass the categories to the page
            ViewBag.Categories = listOfCategeories;

            _productsLogger.LogInformation("Viewing product Details");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public IActionResult addToCart()
        {
            try
            {
                //ProductViewModel p = _productsService.GetProduct(Guid.Parse(HttpContext.Request.Form["id"]));

                //CartViewModel c = new CartViewModel();
                //c.Product = p;
                //c.Email = HttpContext.User.Identity.Name;
                //c.Qty = int.Parse(HttpContext.Request.Form["qty"]);

                //_cartsService.AddCart(c);

                var product = _productsService.GetProduct(Guid.Parse(HttpContext.Request.Form["id"]));
                var cartProduct = _cartsService.GetCart(product.Id).SingleOrDefault(x=>x.Email == HttpContext.User.Identity.Name);

                if(cartProduct == null)
                {
                    cartProduct = new CartViewModel
                    {
                        Product = product,
                        Email = HttpContext.User.Identity.Name,
                        Qty = int.Parse(HttpContext.Request.Form["qty"])
                    };
                    _cartsService.AddCart(cartProduct);
                }
                else
                {
                    int quantity = int.Parse(HttpContext.Request.Form["qty"]) + cartProduct.Qty;
                    _cartsService.UpdateQuantity(cartProduct.Product.Id, quantity);
                }

                TempData["feedback"] = "Product Added to Cart";
                _productsLogger.LogInformation("Product Added To Cart");
            }
            catch(Exception e)
            {
                TempData["warning"] = "Product Not Added To Cart";
                _productsLogger.LogError(e.Message);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User, Admin")]
        public IActionResult removeFromCart(Guid id)
        {
            var email = HttpContext.User.Identity.Name;
            try
            {
                _cartsService.RemoveFromCart(id, email);

                TempData["feedback"] = "Product was deleted successfully";
                _productsLogger.LogInformation("Product Deleted Succesfully");
            }
            catch (Exception e)
            {
                TempData["warning"] = "Product was not Deleted";
                _productsLogger.LogError(e.Message);
            }

            return RedirectToAction("ShoppingCart");
        }

        [Authorize(Roles = "User, Admin")]
        public IActionResult emptyCart()
        {
            var email = HttpContext.User.Identity.Name;
            try
            {
                _cartsService.EmptyCart(email);

                TempData["feedback"] = "Cart Emptied";
                _productsLogger.LogInformation("Cart Emptied");
            }
            catch (Exception e)
            {
                TempData["warning"] = "Cart Not Emptied";
                _productsLogger.LogError(e.Message);
            }

            return RedirectToAction("ShoppingCart");
        }

        [Authorize(Roles = "User, Admin")]
        public IActionResult checkOut()
        {
            var email = HttpContext.User.Identity.Name;
            try
            {
                _ordersService.CheckOut(email);

                TempData["feedback"] = "Check Out Succesfully";
                _productsLogger.LogInformation("Check Out Succeeded");
            }
            catch (Exception e)
            {
                TempData["warning"] = "Check Out Failed";
                _productsLogger.LogInformation(e.Message);
            }

            return RedirectToAction("Index");
        }


        //here details input by the user will be received
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductViewModel data, IFormFile f)
        {
            try
            {
                if(f !=  null)
                {
                    if(f.Length > 0)
                    {
                        string newFilename = Guid.NewGuid() + System.IO.Path.GetExtension(f.FileName);
                        string newFilenameWithAbsolutePath = _env.WebRootPath +  @"\Images\" + newFilename;
                        
                        using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                        {
                            f.CopyTo(stream);
                        }

                        data.ImageUrl = @"\Images\" + newFilename;
                    }
                }

                _productsService.AddProduct(data);

                TempData["feedback"] = "Product was added successfully";
                _productsLogger.LogInformation("Product Created Succesfully");
            }
            catch (Exception ex)
            {
                TempData["warning"] = "Product was not added!";
                _productsLogger.LogError(ex.Message);
            }

           var listOfCategeories = _categoriesService.GetCategories();
           ViewBag.Categories = listOfCategeories;
            return View(data);
        
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _productsService.DeleteProduct(id);
                TempData["feedback"] = "Product was deleted";
                _productsLogger.LogInformation("Product Deleted");
            }
            catch (Exception ex)
            {
                TempData["warning"] = "Product was not deleted"; //Change from ViewData to TempData
                _productsLogger.LogError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DisableOrEnable(Guid id)
        {
            try
            {
                _productsService.DisableOrEnable(id);
                if (_productsService.GetProduct(id).Disable == false)
                {
                    TempData["feedback"] = "Product was disabled";
                    _productsLogger.LogError("Product Disabled");
                }
                else
                {
                    TempData["feedback"] = "Product was Enabled";
                    _productsLogger.LogError("Product Enabled");
                }
            }
            catch (Exception ex)
            {
                TempData["warning"] = "Product was not disabled";
                _productsLogger.LogError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User, Admin")]
        public IActionResult ShoppingCart()
        {
            var cartsList = _cartsService.GetCarts(HttpContext.User.Identity.Name);
            ViewBag.Carts = cartsList;

            var list = new List<ProductViewModel>();

            foreach (var cart in cartsList)
            {
                ProductViewModel p = new ProductViewModel();
                p = cart.Product;
                p.Quantity = cart.Qty;                

                if (list.Count == 0)
                {
                    list.Add(p);
                }
                else
                {
                    int index = 0;
                    int oldQty = 0;
                    bool todelete = false;
                    foreach(var prod in list)
                    {
                        if(prod.Id == p.Id)
                        {
                            index = list.IndexOf(prod);
                            oldQty = list[index].Quantity + cart.Qty;
                            todelete = true;
                            break;
                        }
                        else
                        {
                            oldQty = cart.Qty;
                        }
                    }
                    if (todelete)
                    {
                        list.RemoveAt(index);
                    }
                    p.Quantity = oldQty;
                    list.Add(p);
                }
            }

            ViewBag.productsList = list;
            _productsLogger.LogInformation("Viewing Shopping Cart");

            return View(list);
        }
    }
}
