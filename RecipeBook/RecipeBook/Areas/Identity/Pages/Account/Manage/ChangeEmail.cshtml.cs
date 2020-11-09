using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Areas.Identity.Models;
using RecipeBook.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RecipeBook.Areas.Identity.Pages.Account.Manage
{
    public class ChangeEmailModel : PageModel
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ChangeEmailModel(
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
            [Display(Name = "New email")]
            public string NewEmail { get; set; }

        }

        public IActionResult OnGet()
        {
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

            var user = await _userManager.GetUserAsync(User);
            await SendConfirmationEmail(user, Input.NewEmail);

            TempData["SuccessMessage"] = $"Verification email sent to {Input.NewEmail}";
            return RedirectToPage("./Index");
        }


        private async Task SendConfirmationEmail(AppUser user, string email)
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
    }
}