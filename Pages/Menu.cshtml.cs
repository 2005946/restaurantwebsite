using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebsite.wwwroot.Data;

namespace RestaurantWebsite.Pages
{
    public class MenuModel : PageModel
    {
        private AppDbContext _db;

        public IList<menu> tblMenu { get; private set; }
        [BindProperty]
        public string Search { get; set; }
        public MenuModel(AppDbContext db)
        {
            _db = db;
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

    }
}
