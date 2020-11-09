using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Areas.Identity.Models;
using RecipeBook.Data;
using RecipeBook.Models;
using RecipeBook.Util;

namespace RecipeBook.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = Role.Admin)]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationContext _context;

        public SelectList Roles { get; set; }
        public SelectList Households { get; set; }

        public EditUserModel(UserManager<AppUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public bool IsEmailConfirmed { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Required]
            [Display(Name = "Household")]
            public int Household { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.AppUser
                .Where(x => x.Id == id)
                .Include(x => x.Household)
                .SingleOrDefault();

            if (user == null)
            {
                return NotFound($"Unable to load user '{user.Email}'");
            }

            Roles = Role.GetRoleDropdownList();
            Households = new SelectList(_context.Household.AsNoTracking(), "Id", "Name");
            await LoadUser(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Roles = Role.GetRoleDropdownList();
            Households = new SelectList(_context.Household.AsNoTracking(), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Username);
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            await _userManager.UpdateAsync(user);


            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, Input.Role);

            user.Household = _context.Household.Find(Input.Household);
            _context.AppUser.Update(user);
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = $"Profile for {Input.FirstName} {Input.LastName} has been updated";
            return RedirectToPage("./Index");
        }

        private async Task LoadUser(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var role = (await _userManager.GetRolesAsync(user))[0];

            Input = new InputModel
            {
                Username = userName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role,
                Household = user.Household.Id
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }
    }
}