using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Component : AppTable
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public Recipe Recipe { get; set; }

        public List<Ingredient> Ingredients { get; set; }

        public List<Step> Steps { get; set; }


    }
}
