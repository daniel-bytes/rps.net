using Rps.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rps.Web.Models
{
    public class GameBoardModel
    {
        public string PlayerID { get; set; }
        public long GameID { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public GameTokenModel[] Tokens { get; set; }

        public GameBoardModel()
        {
        }

        public GameBoardModel(Game game, Player currentPlayer)
        {
            this.PlayerID = currentPlayer.ID;
            this.GameID = game.ID;
            this.Rows = game.GameBoard.NumRows;
            this.Cols = game.GameBoard.NumCols;
            this.Tokens = game.GameBoard.GetTokens().Select(token => new GameTokenModel(token, currentPlayer.ID)).ToArray();
        }

        public GameTokenModel Get(int row, int col)
        {
            return this.Tokens.FirstOrDefault(x => x.Row == row && x.Col == col) ?? new GameTokenModel();
        }
    }
}