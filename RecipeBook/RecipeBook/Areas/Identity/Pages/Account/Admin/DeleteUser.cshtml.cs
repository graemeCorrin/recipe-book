using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Areas.Identity.Models;
using RecipeBook.Util;

namespace RecipeBook.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = Role.Admin)]
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteUserModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Account to delete")]
            public string UserEmail { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name="Administrator password")]
            public string AdminPassword { get; set; }
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound($"Unable to load user '{user.Email}'");
            }

            LoadUser(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var admin = await _userManager.GetUserAsync(User);
            if (admin == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var user = await _userManager.FindByEmailAsync(Input.UserEmail);
            if (user == null)
            {
                return NotFound($"Unable to load user '{user.Email}'");
            }


            if (!await _userManager.CheckPasswordAsync(admin, Input.AdminPassword))
            {
                ModelState.AddModelError(string.Empty, "Password not correct.");
                return Page();
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{userId}'.");
            }

            TempData["SuccessMessage"] = $"User {Input.UserEmail} has been deleted";
            return RedirectToPage("./Index");
        }

        private void LoadUser(AppUser user)
        {
            Input = new InputModel
            {
                UserEmail = user.UserName
            };
        }
    }
}