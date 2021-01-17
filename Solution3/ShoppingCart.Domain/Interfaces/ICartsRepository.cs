using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICartsRepository
    {
        Guid AddToCart(Cart c);

        IQueryable<Cart> GetCarts();

        IQueryable<Cart> GetCart(Guid id);

        void DeleteCarts(IQueryable<Cart> carts);

        void EmptyCart(IQueryable<Cart> carts);

        void UpdateQuantity(Guid id, int quantity);
    }
}
