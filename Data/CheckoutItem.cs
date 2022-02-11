using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebsite.Data
{
    public class CheckoutItem
    {
        [Key, Required]
        public int mealID { get; set; }
        [Required]
        public decimal mealPrice { get; set; }
        [Required]
        public string mealName { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
