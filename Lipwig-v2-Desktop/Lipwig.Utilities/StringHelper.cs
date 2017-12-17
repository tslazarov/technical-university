namespace Lipwig.Utilities
{
    public class StringHelper
    {
        public static string EncodeEmailForUrl(string email)
        {
            return email.Replace(".", "%20-%20").Replace("@", "%40");
        }

        public static string DecodeEmailFromUrl(string email)
        {
            return email.Replace(" - ", ".");
        }
    }
}
