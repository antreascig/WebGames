using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;
using WebGames.Models.Dtos;
using WebGames.Helpers;
using WebGames.Libs.Games.GameTypes;

namespace WebGames.Libs
{
    public class GameManager
    {
        public static Dictionary<int, GenericGame> GameDict = new Dictionary<int, GenericGame>();

        public static void Init()
        {
            // initialize the games
        }

        public static int GetActiveGameId()
        {
            return -1;
        }
    }
}