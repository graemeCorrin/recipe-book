using RecipeBook.Areas.Identity.Models;
using RecipeBook.Util;
using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models
{
    public abstract class AppTable
    {
        public int Id { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }


        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = Constants.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime UpdatedDate { get; set; }


        [Display(Name = "Updated By")]
        public AppUser UpdatedBy { get; set; }

        [Display(Name = "Created By")]
        public AppUser CreatedBy { get; set; }
    }
}
