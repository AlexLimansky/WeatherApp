using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuessTheNumber.Utils
{
    public static class Resourses
    {
        public static string PathToStartView = "~/Views/Game/StartView.cshtml";
        public static string PathToGuessView = "~/Views/Game/GuessView.cshtml";
        public static string MoveResultMore = "Your number is bigger than proposed";
        public static string MoveResultLess = "Your number is less than proposed";
        public static string LoginSameEntryAlert = "User with the same name is already in game";
        public static string LoginBlankEntryAlert = "No text was typed!!!";
    }
}