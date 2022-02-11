using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebsite.Data;
using RestaurantWebsite.wwwroot.Data;

namespace RestaurantWebsite.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Registration Input { get; set; }

        private AppDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userInManager;

        public CheckoutCustomer Customer = new CheckoutCustomer();
        public Basket Basket = new Basket();

        public RegisterModel(UserManager<ApplicationUser> um, 
            SignInManager<ApplicationUser> sm,
            AppDbContext db)
        {
            _signInManager = sm;
            _userInManager = um;
            _db = db;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email};
                var result = await _userInManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent:false);
                    await _userInManager.AddToRoleAsync(user, "Member");
                    NewBasket();
                    NewCustomer(Input.Email);
                    await _db.SaveChangesAsync();
                    return RedirectToPage("/Index");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        public void NewBasket()
        {
            var currentBasket = _db.Baskets.FromSqlRaw("SELECT * FROM Baskets")
                .OrderByDescending(b => b.BasketID)
                .FirstOrDefault();
            if(currentBasket == null)
            {
                Basket.BasketID = 1;
            }
            else
            {
                Basket.BasketID = currentBasket.BasketID + 1;
            }
            _db.Baskets.Add(Basket);
        }

        public void NewCustomer(string Email)
        {
            Customer.Email = Email;
            Customer.BasketID = Basket.BasketID;
            _db.CheckoutCustomers.Add(Customer);
        }
    }
}
