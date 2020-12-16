using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        ShoppingCartDbContext _context;
        public ProductsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
         
        public Guid AddProduct(Product p)
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            _context.Products.Add(p);
            _context.SaveChanges();
            return p.Id;
        }

        public void DeleteProduct(Product p)
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            _context.Products.Remove(p);
            _context.SaveChanges(); //this will save permanently into the db
        }

        public void DisableProduct(Guid id)
        {
            var p =GetProduct(id);
            p.Disable = true;
            _context.SaveChanges();
        }


        public Product GetProduct(Guid id)
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            //single or default will return ONE product! or null
            return _context.Products.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Product> GetProducts()
        {
            //ShoppingCartDbContext context = new ShoppingCartDbContext();
            return _context.Products;
        }
    }
}
