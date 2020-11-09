using Microsoft.AspNetCore.Identity;
using RecipeBook.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBook.Areas.Identity.Models
{
    public class AppUser : IdentityUser<int>
    {
        [PersonalData, Required, StringLength(20), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [PersonalData, Required, StringLength(20), Display(Name = "Last Name")]
        public string LastName { get; set; }

        public Household Household { get; set; }


        [NotMapped]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [Display(Name = "Email Confirmed")]
        public override bool EmailConfirmed { get; set; }
    }
}
