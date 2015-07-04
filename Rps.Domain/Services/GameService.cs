using System;
using System.Linq;
using System.Threading.Tasks;
using Rps.Domain.Model;
using Rps.Domain.Repository;
using System.Collections.Generic;
using Rps.Domain.Exceptions;

namespace Rps.Domain.Services
{
    public class GameService
        : IGameService
    {
        private readonly IGameRepository repository;

        public GameService(IGameRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Game> GetGameAsync(long id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<Game> PerformMoveAsync(long id, string playerID, long tokenID, Point point)
        {
            var game = await this.repository.GetAsync(id);
            var attacker = game.GameBoard.GetByID(tokenID);

            if (attacker == null)
            {
                throw new InvalidTokenException(string.Format("Token with ID {0} does not exist.", tokenID));
            }
            else if (attacker.PlayerID != playerID)
            {
                throw new InvalidTokenException(string.Format("Token with ID {0} does not belong to player {1}.", tokenID, playerID));
            }

            // Validate move
            var validMoves = game.GameBoard.GetValidMoves(attacker);

            if (!validMoves.Any())
            {
                throw new InvalidMoveException(string.Format("No moves valid for token {0}.", tokenID));
            }
            else if (!validMoves.Contains(point))
            {
                throw new InvalidMoveException(string.Format("Move {{ {0} }} is not valid.", point));
            }

            // Perform move
            var move = new GameMove(attacker, point);
            
            game.MoveToken(move);

            await repository.SaveAsync(game);

            return game;
        }

        public async Task<Game> PerformComputerMoveAsync(long id)
        {
            var game = await this.repository.GetAsync(id);
            var player = game.GetPlayer(Player.ComputerPlayerID);

            var playerStrategy = player.GetPlayerStrategy();
            var move = playerStrategy.GetNextMove(game.GameBoard, player);

            // Perform move
            game.MoveToken(move);

            await repository.SaveAsync(game);

            return game;
        }

        public async Task<IEnumerable<Game>> GetPlayerActiveGamesAsync(Player player)
        {
            var games = await repository.GetPlayerActiveGamesAsync(player.ID);

            return games;
        }

        public async Task<Game> CreateSinglePlayerGameAsync(Player player, GameProperties properties)
        {
            var gameBoard = GameBoard.New(properties, player, Player.ComputerPlayer);
            var status = GameStatus.CreateActive(player, Player.ComputerPlayer);
            var game = new Game(0, player, Player.ComputerPlayer, status, gameBoard);
            var results = await repository.CreateAsync(game);

            return results;
        }
    }
}
