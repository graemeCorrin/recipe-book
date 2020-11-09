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
    public class ChangeUserEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ChangeUserEmailModel(
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

            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

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
            var existingAccount = await _userManager.FindByEmailAsync(Input.NewEmail);
            if (existingAccount != null)
            {
                ModelState.AddModelError(string.Empty, $"Email {Input.NewEmail} is already in use");
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.CurrentEmail);
            await SendConfirmationEmailAsync(user, Input.NewEmail);

            TempData["SuccessMessage"] = $"Verification email sent to {Input.NewEmail}";
            return RedirectToPage("./Index");
        }


        private async Task SendConfirmationEmailAsync(AppUser user, string email)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, email);
            var callbackUrl = Url.Page(
                "/Account/ConfirmChangeEmail",
                pageHandler: null,
                values: new { userId = userId, code = code, email = email },
                protocol: Request.Scheme);

            await _emailService.ConfirmNewEmailAsync(email, callbackUrl);
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