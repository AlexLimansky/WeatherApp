namespace GuessTheNumberWebAPI.Utils
{
    public static class Resourses
    {
        public static string MoveResultMore = "Your number is bigger than proposed";
        public static string MoveResultLess = "Your number is less than proposed";

        public static string LoginBlankEntryAlert = "No text was typed!!!";

        public static string CookieName = "playerName";
    
        public static string MessageGreeting(string playerName)
        {
            return $"Welcome, {playerName}"; 
        }

        public static string MessageNewGame(string playerName)
        {
            return $"{playerName} started new game)))";
        }

        public static string MessageGameEnded(string playerName)
        {
            return $"{playerName} won this game)))";
        }

    }
}