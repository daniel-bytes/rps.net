using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel = Rps.Domain.Model;

namespace Rps.Web.Models
{
    public class GameMoveResultsModel
    {
        public List<GameMoveResultModel> Results { get; set; }

        public GameMoveResultsModel()
        {
            this.Results = new List<GameMoveResultModel>();
        }

        public void Add(DomainModel.Player player, DomainModel.Game result)
        {
            this.Results.Add(new GameMoveResultModel(player, result));
        }
    }

    public class GameMoveResultModel
    {
        public long GameID { get; set; }
        public DomainModel.GameMoveResultType Result { get; set; }
        public DomainModel.TokenType AttackerTokenType { get; set; }
        public DomainModel.TokenType DefenderTokenType { get; set; }
        public long AttackerTokenID { get; set; }
        public int MoveToX { get; set; }
        public int MoveToY { get; set; }
        public string Message { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public GameTokenModel[] Tokens { get; set; }

        public GameMoveResultModel()
        {
        }

        public GameMoveResultModel(DomainModel.Player player, DomainModel.Game result)
        {
            var defender = result.GameStatus.OtherPlayer;
            var human = player.IsComputerControlled ? defender : player;

            this.GameID = result.ID;
            this.Result = result.GameStatus.CurrentMoveResult;
            this.AttackerTokenType = result.GameStatus.AttackerToken.TokenType;
            this.DefenderTokenType = result.GameStatus.DefenderToken == null ? DomainModel.TokenType.Empty : result.GameStatus.DefenderToken.TokenType;
            this.AttackerTokenID = result.GameStatus.CurrentMove.Token.ID;
            this.MoveToX = result.GameStatus.CurrentMove.MoveTo.X;
            this.MoveToY = result.GameStatus.CurrentMove.MoveTo.Y;
            this.Rows = result.GameBoard.NumRows;
            this.Cols = result.GameBoard.NumCols;
            this.Tokens = result.GameBoard.GetTokens().Select(token => new GameTokenModel(token, human.ID)).ToArray();
            
            switch(result.GameStatus.CurrentMoveResult)
            {
                case DomainModel.GameMoveResultType.AttackerWins:
                    this.Message = string.Format("{0} attacks {1} - {0} Wins!", AttackerTokenType, DefenderTokenType);
                    break;
                case DomainModel.GameMoveResultType.DefenderWins:
                    this.Message = string.Format("{0} attacks {1} - {1} Wins!", AttackerTokenType, DefenderTokenType);
                    break;
                case DomainModel.GameMoveResultType.BothLose:
                    this.Message = string.Format("{0} attacks {1} - Everyone Loses!", AttackerTokenType, DefenderTokenType);
                    break;
                case DomainModel.GameMoveResultType.FlagCapturedByAttacker:
                    this.Message = string.Format("Player {0} Wins!  Game Over.", player.Name);
                    break;
                case DomainModel.GameMoveResultType.AttackerOutOfPieces:
                    this.Message = string.Format("Player {0} ran out of pieces.  Player {1} Wins!  Game Over.", player.Name, defender.Name);
                    break;
                case DomainModel.GameMoveResultType.DefenderOutOfPieces:
                    this.Message = string.Format("Player {0} ran out of pieces.  Player {1} Wins!  Game Over.", defender.Name, player.Name);
                    break;
                case DomainModel.GameMoveResultType.BothOutOfPieces:
                    this.Message = "Its a tie!  Game Over.";
                    break;
            }
        }
    }
}