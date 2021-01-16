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

namespace PresentationWebApp.Controllers
{
    public class ProductsController : Controller
    { 
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICartsService _cartsService;

        private IWebHostEnvironment _env;

        //List<ProductViewModel> productsList = new List<ProductViewModel>();
        List<CartViewModel> cartsList = new List<CartViewModel>();

        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, ICartsService cartsService,
             IWebHostEnvironment env )
        {
            _cartsService = cartsService;
            _productsService = productsService;
            _categoriesService = categoriesService;
            _env = env;
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
            
            return View(listInPage);
            
        }

        //public IActionResult addToList(Guid id, List<ProductViewModel> list)
        //{
        //    var p = _productsService.GetProduct(id);

        //    var _list = list;

        //    _list.Add(p);

        //    return RedirectToAction("Index", _list);
        //}

        [HttpPost]
        public IActionResult Search(int category) //using a form, and the select list must have name attribute = category
        {
            var catList = _categoriesService.GetCategories();
            ViewBag.Categories = catList;

            var list = _productsService.GetProducts(category);
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

            return View();
        }

        public IActionResult addToCart(Guid id)
        {
            try
            {
                ProductViewModel p = _productsService.GetProduct(id);

                CartViewModel c = new CartViewModel();
                c.Id = Guid.NewGuid();
                c.Product = p;
                c.Email = HttpContext.User.Identity.Name;
                c.Qty = 1;

                _cartsService.AddCart(c);

                TempData["feedback"] = "Cart was added successfully";
            }catch(Exception e)
            {
                TempData["warning"] = "Cart was not Added";
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
            }
            catch (Exception ex)
            {
                //log error
                TempData["warning"] = "Product was not added!";
            }

           var listOfCategeories = _categoriesService.GetCategories();
           ViewBag.Categories = listOfCategeories;
            return View(data);
        
        } //fiddler, burp, zap, postman

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _productsService.DeleteProduct(id);
                TempData["feedback"] = "Product was deleted";
            }
            catch (Exception ex)
            {
                //log your error 

                TempData["warning"] = "Product was not deleted"; //Change from ViewData to TempData
            }

            return RedirectToAction("Index");
        }

        public IActionResult ShoppingCart(List<ProductViewModel> list)
        {            
            return View(list);
        }
    }
}
