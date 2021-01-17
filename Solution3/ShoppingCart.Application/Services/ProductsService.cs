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

        public void DisableOrEnable(Guid id)
        {
            _productsRepo.DisableOrEnableProduct(id);
        }

        public ProductViewModel GetProduct(Guid id)
        {
            var myProduct = _productsRepo.GetProduct(id);
            var result = _mapper.Map<ProductViewModel>(myProduct);
            return result;

        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            var products = _productsRepo.GetProducts().ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);

            return products;
        }

        public IQueryable<ProductViewModel> GetProducts(int category)
        {
            var list = _productsRepo.GetProducts().Where(x => x.CategoryId == category)
                       .ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider);
            return list;

        }



    }
}
