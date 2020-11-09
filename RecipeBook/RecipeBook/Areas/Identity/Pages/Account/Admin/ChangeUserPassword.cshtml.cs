using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Areas.Identity.Models;
using RecipeBook.Services;
using RecipeBook.Util;

namespace RecipeBook.Areas.Identity.Pages.Account.Admin
{
    [Authorize(Roles = Role.Admin)]
    public class ChangeUserPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ChangeUserPasswordModel(
            UserManager<AppUser> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Current email")]
            public string CurrentEmail { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
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
            var user = await _userManager.FindByEmailAsync(Input.CurrentEmail);
            if (user == null)
            {
                return NotFound($"Unable to load user '{user.Email}'");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page("/Account/ResetPassword",
                pageHandler: null,
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailService.ResetPasswordAsync(Input.CurrentEmail, callbackUrl);

            TempData["SuccessMessage"] = $"Password reset email sent to {Input.CurrentEmail}";
            return RedirectToPage("./Index");
        }

        private void LoadUser(AppUser user)
        {
            Input = new InputModel
            {
                CurrentEmail = user.UserName
            };
        }
    }
}