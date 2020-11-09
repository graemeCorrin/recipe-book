
namespace RecipeBook.Util
{
    public class EmailOptions
    {
        public string SmtpHost { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderPassword { get; set; }
    }
}
