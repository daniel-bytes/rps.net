using System;
using System.Threading.Tasks;
using Rps.Domain.Model;
using System.Collections.Generic;

namespace Rps.Domain.Services
{
    public interface IGameService
    {
        Task<Game> GetGameAsync(long id);
        Task<GameMoveResult> PerformMoveAsync(long id, string playerID, long tokenID, Point point);
        Task<Game> CreateSinglePlayerGameAsync(Player player, GameProperties properties);
        Task<IEnumerable<Game>> GetPlayerActiveGamesAsync(Player player);
    }
}
