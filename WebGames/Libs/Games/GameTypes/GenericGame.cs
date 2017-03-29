using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebGames.Models;

namespace WebGames.Libs.Games.GameTypes
{
    public abstract class GenericGame
    {
        public abstract int GetScoreLimit(GameModel Game);

        public abstract object GetDefaultMetaData();

        public abstract double ComputeScore(UserGameScore UserScore);

        public abstract object GetMetadata(string MetadataJSON);

        public abstract object GetClientViewModel(string MetadataJSON);
    }
}
