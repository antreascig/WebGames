using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public class Game1MetaData
    {
        public double Multiplier { get; set; }
    }

    public class Game1ViewModel
    {

    }

    public class Game1 : GenericGame
    {
        public override double ComputeScore(UserGameScore UserScore)
        {
            if (UserScore == null || UserScore.Game == null) return -1;

            var Metadata = GetMetadata(UserScore.Game.MetadataJSON);

            if (Metadata == null || !(Metadata is Game1MetaData) ) return -1;

            return ((Game1MetaData)Metadata).Multiplier * UserScore.Score;
        }

        public override object GetClientViewModel(string MetadataJSON)
        {
            return new Game1ViewModel();
        }

        public override object GetDefaultMetaData()
        {
            return new Game1MetaData()
            {
                Multiplier = 1
            };
        }

        public override object GetMetadata(string MetadataJSON)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Game1MetaData>(MetadataJSON ?? "{}");
            }
            catch
            {
                return null;
            }
        }

        public override int GetScoreLimit(GameModel Game)
        {
            throw new NotImplementedException();
        }
    }
}