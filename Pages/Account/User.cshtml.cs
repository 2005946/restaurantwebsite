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
    public class UserModel : PageModel
    {

        private AppDbContext _db;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public IList<UserOrder> Items { get; private set; }
        public UserModel(AppDbContext db, SignInManager<ApplicationUser> sm, UserManager<ApplicationUser> um)
        {
            _db = db;
            _signInManager = sm;
            _userManager = um;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Index");
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Items = _db.UserOrders.FromSqlRaw("SELECT OrderItems.OrderNo, tblMenu.mealName, OrderItems.Quantity FROM OrderItems INNER JOIN OrderHistories ON OrderItems.OrderNo = OrderHistories.OrderNo INNER JOIN tblMenu ON OrderItems.StockID = tblMenu.mealID WHERE Email='{0}'",_userManager.GetEmailAsync(user).ToString()).ToList();
        }

    }
}
