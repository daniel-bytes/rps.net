using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Core.Config
{
    public class ApplicationConfig
        : IApplicationConfig
    {
        public string GetConnectionString(string name)
        {
            return GetConnectionStringSettings(name).ConnectionString;
        }

        public string GetConnectionProvider(string name)
        {
            return GetConnectionStringSettings(name).ProviderName;
        }

        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        private ConnectionStringSettings GetConnectionStringSettings(string name)
        {
            var connString = ConfigurationManager.ConnectionStrings[name];

            if (connString == null)
            {
                throw new ArgumentException("Connection string " + name + " does not exist in the currecnt configuration.", "name");
            }

            return connString;
        }


    }
}
