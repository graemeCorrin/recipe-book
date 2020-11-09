using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Data;
using RecipeBook.Models;

namespace RecipeBook.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;

        public List<Household> Households { get; set; }

        public IndexModel(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Households = await _context.Household.Include(x => x.AppUsers).AsNoTracking().ToListAsync();
            return Page();
        }
    }
}