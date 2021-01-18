using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Product Required")]
        public virtual ProductViewModel Product { get; set; }
        [Required(ErrorMessage = "Email Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Quantity Required")]
        [Range(typeof(int), "0", "9999", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]
        public int Qty { get; set; }
    }
}
