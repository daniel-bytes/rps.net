using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Core.Config
{
    public interface IApplicationConfig
    {
        string GetConnectionString(string name);
        string GetConnectionProvider(string name);
        string GetAppSetting(string key);
    }
}
