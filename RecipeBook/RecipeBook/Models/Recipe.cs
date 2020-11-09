using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace RecipeBook.Models
{
    public class Recipe : AppTable
    {

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Source { get; set; }
        
        public Household Household { get; set; }

        public List<Component> Components { get; set; }

        public List<Book> Books { get; set; }

        public List<Tag> Tags { get; set; }

    }
}
