using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebsite.wwwroot.Data
{
    public class menu
    {
        [Key]
        public int mealID { get; set; }
        [StringLength(25)]
        public string mealName { get; set; }
        [StringLength(100)]
        public string mealIngredients { get; set; }
        public decimal mealPrice { get; set; }
        public bool mealIsGlutenFree { get; set; }
        public bool mealIsVegan { get; set; }
        public bool mealContainsNuts { get; set; }
        [StringLength(250)]
        public string mealDescription { get; set; }
        [StringLength(250)]
        public string ImageDescription { get; set; }
        public byte[] ImageData { get; set; }
        public Boolean Active { get; set; }

    }
}
