using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel = Rps.Domain.Model;

namespace Rps.Web.Models
{
    public class GameMoveResultModel
    {
        public long GameID { get; set; }
        public DomainModel.GameMoveResultType Result { get; set; }
        public DomainModel.TokenType AttackerTokenType { get; set; }
        public DomainModel.TokenType DefenderTokenType { get; set; }
        public string Message { get; set; }

        public GameMoveResultModel()
        {
        }

        public GameMoveResultModel(long gameID, DomainModel.Player player, DomainModel.GameMoveResult result)
        {
            this.GameID = gameID;
            this.Result = result.ResultType;
            this.AttackerTokenType = result.Attacker.TokenType;
            this.DefenderTokenType = result.Defender == null ? DomainModel.TokenType.Empty : result.Defender.TokenType;
            
            switch(result.ResultType)
            {
                case DomainModel.GameMoveResultType.AttackerWins:
                    this.Message = string.Format("{0} attacks {1} - {0} Wins!", result.Attacker.TokenType, result.Defender.TokenType);
                    break;
                case DomainModel.GameMoveResultType.DefenderWins:
                    this.Message = string.Format("{0} attacks {1} - {1} Wins!", result.Attacker.TokenType, result.Defender.TokenType);
                    break;
                case DomainModel.GameMoveResultType.BothLose:
                    this.Message = string.Format("{0} attacks {1} - Everyone Loses!", result.Attacker.TokenType, result.Defender.TokenType);
                    break;
                case DomainModel.GameMoveResultType.GameOver:
                    this.Message = string.Format("{0} Wins!  Game Over.", result.Winner.Name);
                    break;
            }
        }
    }
}