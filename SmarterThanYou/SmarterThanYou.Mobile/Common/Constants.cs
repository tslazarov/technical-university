namespace SmarterThanYou.Mobile.Common
{
    public static class Constants
    {
        public static string BaseUri = "http://670a5c48.ngrok.io";
        public static string MediaType = "application/json";

        public static string ApiLogin = "api/account/login";
        public static string ApiRegister = "api/account/register";
        public static string ApiQuestion = "api/questions/random";
        public static string ApiSubmitScore = "api/scoreboard/submit";
        public static string ApiScoreboard = "api/scoreboard/all";

        public static string LoginErrorMessage = "Невалидно име или парола";
        public static string RegisterErrorMessage = "Вече съществува такъв потребител";
        public static string HighScoreMessage = "НОВ ЛИЧЕН РЕКОРД";
    }
}
