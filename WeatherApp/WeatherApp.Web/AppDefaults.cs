namespace WeatherApp.Web
{
    public static class AppDefaults
    {
        public const string DbKey = "db";
        public const string MySqlSelector = "mysql";
        public const string SqlSelector = "sql";

        public const string ErrorDefaultPath = "/Weather/Error";
        public const string LoginDefaultPath = "/Account/LogIn";
        public const string LogoutDefaultPath = "/Account/LogOut";
        public const string ResourcesDefaultPath = "Resources";

        public static readonly string[] CulturesCollection = { "en", "ru" };

        public static readonly string[] RolesCollection = { "user", "admin" };
    }

}
