using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public enum GameMoveResultType
    {
        Nothing = 0,
        TokenMove = 1,
        AttackerWins = 2,
        DefenderWins = 3,
        BothLose = 4,
        GameOver = 5
    }

    public struct GameMoveResult
    {
        public GameMoveResultType ResultType { get; private set; }
        public Token Attacker { get; private set; }
        public Token Defender { get; private set; }
        public Player Winner { get; private set; }

        public GameMoveResult(
            GameMoveResultType resultType,
            Token attacker,
            Token defender)
            : this(resultType, attacker, defender, null)
        {
        }

        public GameMoveResult(
            GameMoveResultType resultType,
            Token attacker,
            Token defender,
            Player winner)
            : this()
        {
            this.ResultType = resultType;
            this.Attacker = attacker;
            this.Defender = defender;
            this.Winner = winner;
        }

        public GameMoveResult WithWinner(Player winner)
        {
            return new GameMoveResult(this.ResultType, this.Attacker, this.Defender, winner);
        }
    }
}
