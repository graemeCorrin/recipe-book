﻿using Microsoft.AspNetCore.Identity;

namespace RecipeBook.Areas.Identity.Models
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole() { }

        public AppRole(string name)
        {
            Name = name;
        }
    }
}
