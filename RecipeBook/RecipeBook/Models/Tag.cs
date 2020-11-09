using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Tag : AppTable
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public List<Recipe> Recipes { get; set; }

    }
}
