using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.AutoMapper;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Data.Repositories;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class ProductsService : IProductsService
    {
        private IMapper _mapper;
        private IProductsRepository _productsRepo;
        public ProductsService(IProductsRepository productsRepository
           ,  IMapper mapper
            )
        {
            _mapper = mapper;
            _productsRepo = productsRepository;
        }

        public void AddProduct(ProductViewModel product)
        {
            //changing this using automapper later on

            //Converting from
            //ProductViewModel >> Product
            /*     Product newProduct = new Product()
                 {
                     Description = product.Description,
                     Name = product.Name,
                     Price = product.Price,
                     CategoryId = product.Category.Id,
                     ImageUrl = product.ImageUrl
                 };

                 _productsRepo.AddProduct(newProduct);
            */

            var myProduct = _mapper.Map<Product>(product);
            myProduct.Category = null;

            _productsRepo.AddProduct(myProduct);

        }

        public void DeleteProduct(Guid id)
        {
            var pToDelete = _productsRepo.GetProduct(id);

            if (pToDelete != null)
            {
                _productsRepo.DeleteProduct(pToDelete);
            }
            
        }

        public ProductViewModel GetProduct(Guid id)
        {
            //AutoMapper


           

            var myProduct = _productsRepo.GetProduct(id);
            var result = _mapper.Map<ProductViewModel>(myProduct);
            return result;


         /*  ProductViewModel myModel = new ProductViewModel();
            myModel.Description = myProduct.Description;
            myModel.ImageUrl = myProduct.ImageUrl;
            myModel.Name = myProduct.Name;
            myModel.Price = myProduct.Price;
            myModel.Id = myProduct.Id;
            myModel.Category = new CategoryViewModel()
            {
                Id = myProduct.Category.Id,
                Name = myProduct.Category.Name
            };
            return myModel;
         */

        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            //to check whether this works
            //demonstrate the alternative way with ProjectTo...


            var products = _productsRepo.GetProducts().ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);

            return products;
            //Domain >> ViewModels

            //to be implemented using AutoMapper
         /*   var list = from p in _productsRepo.GetProducts()
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name },
                           ImageUrl = p.ImageUrl
                       };
            return list;
         */

        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = from p in _productsRepo.GetProducts().Where(x => x.Category.Id == category)
                       select new ProductViewModel()
                       {
                           Id = p.Id,
                           Description = p.Description,
                           Name = p.Name,
                           Price = p.Price,
                           Category = new CategoryViewModel() { Id = p.Category.Id, Name = p.Category.Name },
                           ImageUrl = p.ImageUrl
                       };
            return list;
        }



    }
}
