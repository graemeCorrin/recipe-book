
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace RecipeBook.Util
{
    public class Role
    {
        public const string ReadOnly = "ReadOnly";
        public const string Standard = "Standard";
        public const string Admin = "Admin";

        public const string ReadOnlyDescription = "Read Only User";
        public const string StandardDescription = "Standard User";
        public const string AdminDescription = "Administrator";

        public const string AdminStandard = Admin + "," + Standard;
        public const string Any = Admin + "," + Standard + "," + ReadOnly;

        static public SelectList GetRoleDropdownList()
        {
            var roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem { Value = ReadOnly, Text = ReadOnlyDescription });
            roleList.Add(new SelectListItem { Value = Standard, Text = StandardDescription });
            roleList.Add(new SelectListItem { Value = Admin, Text = AdminDescription });
            return new SelectList(roleList, "Value", "Text");
        }
    }
}
