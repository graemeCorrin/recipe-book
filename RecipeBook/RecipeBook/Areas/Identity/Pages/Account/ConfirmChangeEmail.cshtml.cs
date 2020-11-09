using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Areas.Identity.Models;

namespace RecipeBook.Areas.Identity.Pages.Account
{
    public class ConfirmChangeEmailModel : PageModel
    {

        private readonly UserManager<AppUser> _userManager;

        public ConfirmChangeEmailModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string email)
        {
            if (userId == null || code == null || email == null)
            {
                return RedirectToPage("/Index");
            }

            var existingAccount = await _userManager.FindByEmailAsync(email);
            if (existingAccount != null)
            {
                throw new InvalidOperationException($"Error: email {email} already in use for user with ID");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error changing email for user with ID '{userId}':");
            }

            //TODO: If an exception is thrown here, the user will be unable to login
            user.UserName = email;
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return Page();

        }
    }
}