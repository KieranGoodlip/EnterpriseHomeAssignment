using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Product")]
        public Guid Product_FK { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Range(typeof(int), "0", "9999", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]
        public int Qty { get; set; }
    }
}
