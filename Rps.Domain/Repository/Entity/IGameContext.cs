using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Repository.Entity
{
    public interface IGameContext
        : IDisposable
    {
        IDbSet<Game> Games { get; }
        IDbSet<Token> Tokens { get; }

        Task<int> SaveChangesAsync();
    }
}
