using System;

namespace RecipeBook.Services
{
    public class PasswordGeneratorService : IPasswordGeneratorService
    {
        private readonly Random random;

        public PasswordGeneratorService()
        {
            random = new Random();
        }

        public string GeneratePassword(
            int lengthOfPassword,
            bool includeLowercase = true,
            bool includeUppercase = true,
            bool includeNumeric = true,
            bool includeSpecial = true)
        {

            const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
            const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMERIC_CHARACTERS = "0123456789";
            const string SPECIAL_CHARACTERS = "!@#$%^&*";

            string characterSet = "";
            char[] password = new char[lengthOfPassword];
            int i = 0;

            try
            {
                if (includeLowercase)
                {
                    characterSet += LOWERCASE_CHARACTERS;
                    password[i] = LOWERCASE_CHARACTERS[random.Next(LOWERCASE_CHARACTERS.Length)];
                    i++;
                }
                if (includeUppercase)
                {
                    characterSet += UPPERCASE_CHARACTERS;
                    password[i] = UPPERCASE_CHARACTERS[random.Next(UPPERCASE_CHARACTERS.Length)];
                    i++;
                }
                if (includeNumeric)
                {
                    characterSet += NUMERIC_CHARACTERS;
                    password[i] = NUMERIC_CHARACTERS[random.Next(NUMERIC_CHARACTERS.Length)];
                    i++;
                }
                if (includeSpecial)
                {
                    characterSet += SPECIAL_CHARACTERS;
                    password[i] = SPECIAL_CHARACTERS[random.Next(SPECIAL_CHARACTERS.Length)];
                    i++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Password length too short");
            }

            if (characterSet.Length == 0)
            {
                throw new Exception("Password must contain at least one type of character");
            }

            while (i < lengthOfPassword)
            {
                password[i] = characterSet[random.Next(characterSet.Length)];
                i++;
            }

            return new string(password);
        }

    }
}
