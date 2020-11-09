using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Ingredient : AppTable
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public float Quantity { get; set; }
        
        [Required]
        public Unit Unit { get; set; }

        public Component Component { get; set; }

    }
}
