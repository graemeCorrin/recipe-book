using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Step : AppTable
    {

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        
        public Component Component { get; set; }

    }
}
