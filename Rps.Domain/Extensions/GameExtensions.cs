using Rps.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Extensions
{
    public static class GameExtensions
    {
        public static bool IsMovable(this TokenType tokenType)
        {
            return tokenType == TokenType.Rock ||
                    tokenType == TokenType.Paper ||
                    tokenType == TokenType.Scissor;
        }

        public static bool IsGameOver(this GameMoveResultType result)
        {
            return result == GameMoveResultType.AttackerOutOfPieces ||
                    result == GameMoveResultType.BothOutOfPieces ||
                    result == GameMoveResultType.DefenderOutOfPieces ||
                    result == GameMoveResultType.FlagCapturedByAttacker;
        }

        public static Player GetWinner(this GameMoveResultType result, Player attacker, Player defender)
        {
            switch(result)
            {
                case GameMoveResultType.AttackerOutOfPieces:
                    return defender;
                case GameMoveResultType.DefenderOutOfPieces:
                    return attacker;
                case GameMoveResultType.FlagCapturedByAttacker:
                    return attacker;
                default:
                    return null;
            }
        }
    }
}
