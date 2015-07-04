using System;
using System.Collections.Generic;
using System.Linq;
using Rps.Domain.Exceptions;
using Rps.Domain.Extensions;

namespace Rps.Domain.Model
{
    public class Game
    {
        public long ID { get; private set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public GameStatus GameStatus { get; private set; }
        public GameBoard GameBoard { get; private set; }

        public Game(long id,
                    Player player1,
                    Player player2,
                    GameStatus gameStatus,
                    GameBoard gameBoard)
        {
            this.ID = id;
            this.Player1 = player1;
            this.Player2 = player2;
            this.GameStatus = gameStatus;
            this.GameBoard = gameBoard;
        }

        public Player GetPlayer(string id)
        {
            if (Player1.ID == id)
            {
                return Player1;
            }
            else if (Player2.ID == id)
            {
                return Player2;
            }

            throw new ArgumentException("Invalid player ID " + id + ".", "id");
        }

        public Player GetOtherPlayer(string id)
        {
            if (Player1.ID == id)
            {
                return Player2;
            }
            else if (Player2.ID == id)
            {
                return Player1;
            }

            throw new ArgumentException("Invalid player ID " + id + ".", "id");
        }

        public void MoveToken(GameMove move)
        {
            Token token = move.Token;
            Point moveTo = move.MoveTo;

            if (token.TokenType == TokenType.Empty ||
                token.TokenType == TokenType.Flag ||
                token.TokenType == TokenType.Bomb)
            {
                throw new InvalidMoveException(string.Format("Invalid move.  Token type {0} cannot be moved.", token.TokenType));
            }

            var defender = this.GameBoard.Get(moveTo);

            var result = token.Attack(defender);

            switch (result)
            {
                case GameMoveResultType.AttackerWins:
                    this.GameBoard.Remove(defender.Point);
                    token.SetPoint(moveTo);
                    break;
                case GameMoveResultType.DefenderWins:
                    this.GameBoard.Remove(token.Point);
                    break;
                case GameMoveResultType.BothLose:
                    this.GameBoard.Remove(token.Point);
                    this.GameBoard.Remove(defender.Point);
                    break;
                case GameMoveResultType.TokenMove:
                    token.SetPoint(moveTo);
                    break;
            }

            if (result != GameMoveResultType.FlagCapturedByAttacker)
            {
                var p1ActiveTokens = this.GameBoard.GetTokens().Count(x => x.PlayerID == Player1.ID && x.IsMovable());
                var p2ActiveTokens = this.GameBoard.GetTokens().Count(x => x.PlayerID == Player2.ID && x.IsMovable());

                if (p1ActiveTokens == 0)
                {
                    if (p2ActiveTokens == 0)
                    {
                        result = GameMoveResultType.BothOutOfPieces;
                    }
                    else
                    {
                        result = GameMoveResultType.AttackerOutOfPieces;
                    }
                }
                else if (p2ActiveTokens == 0)
                {
                    result = GameMoveResultType.DefenderOutOfPieces;
                }
            }

            this.GameStatus = GameStatus.CreateForMove(this.GameStatus, move, result, token, defender);
        }
    }
}
