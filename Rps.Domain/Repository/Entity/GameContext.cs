using Rps.Domain.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Repository.Entity
{
    public class GameContext
        : DbContext, IGameContext
    {
        public GameContext(IDomainConfig config)
            : base(config.DefaulConnectionString)
        {
        }

        public GameContext()
            : base(DomainConfig.ConnStringKey_DefaultConnection)
        {
        }

        public IDbSet<Game> Games { get; set; }
        public IDbSet<Token> Tokens { get; set; }
    }
}
