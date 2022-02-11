using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebsite.Data;
using RestaurantWebsite.wwwroot.Data;
using Microsoft.AspNetCore.Identity;

namespace RestaurantWebsite.Pages
{
    public class MenuModel : PageModel
    {
        private AppDbContext _db;

        public IList<menu> tblMenu { get; private set; }
        [BindProperty]
        public string Search { get; set; }
        public readonly UserManager<ApplicationUser> _userManager;
        public MenuModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public void OnGet()
        {
            tblMenu = _db.tblMenu.FromSqlRaw("SELECT * FROM tblMenu ORDER BY Active DESC").ToList();
        }

        public IActionResult OnPostSearch()
        {
            tblMenu = _db.tblMenu.FromSqlRaw("SELECT * FROM tblMenu WHERE mealName LIKE '" + Search + "%' ORDER BY Active DESC").ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostBuyAsync(int mealID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var item = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE StockID = {0} AND BasketID = {1}", mealID, customer.BasketID)
                .ToList().FirstOrDefault();

            if(item == null)
            {
                BasketItem newItem = new BasketItem
                {
                    BasketID = customer.BasketID,
                    StockID = mealID,
                    Quantity = 1
                };
                _db.BasketItems.Add(newItem);
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity += 1;
                _db.Attach(item).State = EntityState.Modified;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Unable to add item to basket", e);
                }

            }
            return RedirectToPage();
        }

    }
}
