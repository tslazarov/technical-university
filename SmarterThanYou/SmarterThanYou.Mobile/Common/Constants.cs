namespace SmarterThanYou.Mobile.Common
{
    public static class Constants
    {
        public static string BaseUri = "http://75b2b70f.ngrok.io";
        public static string MediaType = "application/json";

        public static string ApiLogin = "api/account/login";
        public static string ApiRegister = "api/account/register";
        public static string ApiQuestion = "api/questions/random";

        public static string LoginErrorMessage = "Username or password is incorrect";
        public static string RegisterErrorMessage = "A user with such username exists";
    }
}
