using RecipeBook.Areas.Identity.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models
{
    public class Household : AppTable
    {
        [Required, StringLength(20), Display(Name = "Household")]
        public string Name { get; set; }

        [Display(Name = "Members")]
        public List<AppUser> AppUsers { get; set; }

        public List<Book> Books { get; set; }

        public List<Recipe> Recipes { get; set; }
    }
}
