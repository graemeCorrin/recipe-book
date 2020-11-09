using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Book : AppTable
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public List<Recipe> Recipes { get; set; }

        [Required]
        public Household Household { get; set; }

    }
}
