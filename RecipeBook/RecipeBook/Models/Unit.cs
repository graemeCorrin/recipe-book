using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Unit : AppTable
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }

    }
}
