using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public virtual ProductViewModel Product { get; set; }
        public string Email { get; set; }
        public int Qty { get; set; }
    }
}
