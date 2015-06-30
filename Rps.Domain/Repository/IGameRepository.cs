using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rps.Domain.Model;

namespace Rps.Domain.Repository
{
    public interface IGameRepository
    {
        Task<Game> GetAsync(long id);
        Task<IEnumerable<Game>> GetPlayerActiveGamesAsync(string playerID);
        Task<Game> CreateAsync(Game game);
        Task SaveAsync(Game game);
    }
}
