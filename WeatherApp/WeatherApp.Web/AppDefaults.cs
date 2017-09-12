namespace WeatherApp.Web
{
    public static class AppDefaults
    {
        public const string DB_KEY = "db";
        public const string MY_SQL_SELECTOR = "mysql";
        public const string SQL_SELECTOR = "sql";

        public const string ERROR_DEFAULT_PATH = "/Weather/Error";
        public const string LOGIN_DEFAULT_PATH = "/Account/LogIn";
        public const string LOGOUT_DEFAULT_PATH = "/Account/LogOut";
        public const string RESOURCES_DEFAULT_PATH = "Resources";

        public static readonly string[] CulturesCollection = { "en", "ru" };

        public static readonly string[] RolesCollection = { "user", "admin" };
    }
}