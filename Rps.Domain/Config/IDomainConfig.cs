using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Config
{
    public interface IDomainConfig
    {
        string DefaulConnectionString { get; }
        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
    }
}
