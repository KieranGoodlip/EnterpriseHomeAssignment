using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CartsRepository : ICartsRepository
    {
        ShoppingCartDbContext _context;

        public CartsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }

        public Guid AddToCart(Cart c)
        {
            try
            {
                _context.Carts.Add(c);
                _context.SaveChanges();
                return c.Id;
            }
            catch (Exception e)
            {
                return Guid.NewGuid();
            }
        }

        public void DeleteCarts(IQueryable<Cart> carts)
        {
            foreach(var cart in carts)
            {
                _context.Carts.Remove(cart);
            }

            _context.SaveChanges();
        }

        public void EmptyCart(IQueryable<Cart> carts)
        {
            foreach (var cart in carts)
            {
                _context.Carts.Remove(cart);
            }

            _context.SaveChanges();
        }

        public IQueryable<Cart> GetCart(Guid id)
        {
            var cart = _context.Carts.Where(x => x.Product_FK == id);
            return cart;
        }

        public IQueryable<Cart> GetCarts()
        {
            return _context.Carts;
        }

        public void UpdateQuantity(Guid id, int quantity, string email)
        {
            var cart = _context.Carts.SingleOrDefault(x => x.Product_FK == id && x.Email == email);
            cart.Qty = quantity;
            _context.SaveChanges();
        }
    }
}
