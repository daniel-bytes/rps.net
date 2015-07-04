using Rps.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.AI
{
    public class BasicPlayerStrategyV1
        : IPlayerStrategy
    {
        public GameMove GetNextMove(GameBoard gameBoard, Player player)
        {
            // most naive AI ever
            var random = Core.Utility.Random.New();
            var validTokens = gameBoard.GetTokens()
                                       .Where(x => x.PlayerID == player.ID && x.IsMovable())
                                       .Select(x => new { Token = x, Moves = gameBoard.GetValidMoves(x).ToList() })
                                       .Where(x => x.Moves.Any())
                                       .ToList();
            
            if (!validTokens.Any())
            {
                throw new Exceptions.InvalidMoveException("No available moves for player " + player.Name + ".");
            }

            var token = validTokens[random.Next(validTokens.Count)];
            var point = token.Moves[random.Next(token.Moves.Count)];

            return new GameMove(token.Token, point);
        }
    }
}
