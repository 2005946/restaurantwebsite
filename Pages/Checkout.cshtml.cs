using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebsite.Data;
using RestaurantWebsite.wwwroot.Data;
using Stripe;

namespace RestaurantWebsite.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public IList<CheckoutItem> Items { get; private set; }
        public OrderHistory Order = new OrderHistory();

        public decimal Total = 0;
        public long AmountPayabale = 0;

        public CheckoutModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT tblMenu.mealID, tblMenu.mealPrice, tblMenu.mealName, BasketItems.BasketID, BasketItems.Quantity "+
                "FROM tblMenu INNER JOIN BasketItems ON tblMenu.mealID = BasketItems.StockID "+
                "WHERE BasketID = {0}", customer.BasketID).ToList();

            Total = 0;
            foreach(var item in Items)
            {
                Total += (item.mealPrice * item.Quantity);
            }
            AmountPayabale = (long)(Total * 100);
        }

        public async Task Checkout()
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories").OrderByDescending(b => b.OrderNo).FirstOrDefault();
            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }
            var user = await _userManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var basketItems = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE BasketID = {0}", customer.BasketID).ToList();
            foreach (var item in basketItems)
            {
                Data.OrderItem oi = new Data.OrderItem
                {
                    OrderNo = Order.OrderNo,
                    StockID = item.StockID,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(oi);
                _db.BasketItems.Remove(item);
            }
            await _db.SaveChangesAsync();
        }
        public async Task<IActionResult> OnPostMoreAsync(int StockID)
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories").OrderByDescending(b => b.OrderNo).FirstOrDefault();
            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }
            var user = await _userManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var basketItems = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE BasketID = {0}", customer.BasketID).ToList();

            var basketItem = await _db.BasketItems.FindAsync(StockID, customer.BasketID);
            basketItem.Quantity += 1;
            _db.Attach(basketItem).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Item {basketItem.StockID} could not be updated", e);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLessAsync(int StockID)
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories").OrderByDescending(b => b.OrderNo).FirstOrDefault();
            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }
            var user = await _userManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var basketItems = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE BasketID = {0}", customer.BasketID).ToList();

            var basketItem = await _db.BasketItems.FindAsync(StockID, customer.BasketID);
            if (basketItem.Quantity == 1){
                try {
                    _db.BasketItems.FromSqlRaw("DELETE FROM BasketItems WHERE StockID = {0} AND BasketID = {1}", StockID, customer.BasketID).ToList();
                }
                catch
                {
                    return RedirectToPage("/Checkout");
                }
                
            }
            else if (basketItem.Quantity >=1)
            {
                basketItem.Quantity -= 1;        
            }
            _db.Attach(basketItem).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Item {basketItem.StockID} could not be updated", e);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostCharge(string stripeEmail, string stripeToken, long amount)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = amount,
                Description = "The Logo Cafe Charge",
                Currency = "GBP",
                Customer = customer.Id
            });
            Checkout().Wait();
            return RedirectToPage("/Index");
        }
    }
}
