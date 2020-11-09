
namespace RecipeBook.Services
{
    public interface IPasswordGeneratorService
    {
        string GeneratePassword(
            int lengthOfPassword,
            bool includeLowercase = true,
            bool includeUppercase = true,
            bool includeNumeric = true,
            bool includeSpecial = true);
    }
}
