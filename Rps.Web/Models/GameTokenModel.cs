using Rps.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Rps.Web.Models
{
    [DebuggerDisplay("{Row} {Col} {TokenType}")]
    public class GameTokenModel
    {
        public GameTokenType TokenType { get; set; }
        public long TokenID { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsPlayerToken { get; set; }

        public GameTokenModel()
        {
        }

        public GameTokenModel(Token token, string userID)
        {
            TokenID = token.ID;
            Row = token.Row;
            Col = token.Col;
            TokenType = userID == token.PlayerID
                            ? (GameTokenType)((int)token.TokenType)
                            : GameTokenType.OtherPlayer;
            IsPlayerToken = TokenType != GameTokenType.Empty && TokenType != GameTokenType.OtherPlayer;
        }

        public override string ToString()
        {
            switch(TokenType)
            {
                case GameTokenType.Empty:
                    return string.Empty;
                case GameTokenType.OtherPlayer:
                    return "X";
                default:
                    return TokenType.ToString();
            }
        }
    }
}