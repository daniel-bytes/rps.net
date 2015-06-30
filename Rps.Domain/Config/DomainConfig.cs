using Rps.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Config
{
    public class DomainConfig
        : IDomainConfig
    {
        public const string ConnStringKey_DefaultConnection = "RpsContext";
        public const string AppSettingsKey_GoogleClientId = "Google.ClientId";
        public const string AppSettingsKey_GoogleClientSecret = "Google.ClientSecret";

        private readonly IApplicationConfig config;

        public DomainConfig(IApplicationConfig config)
        {
            this.config = config;
        }

        public string DefaulConnectionString
        {
            get
            {
                return config.GetConnectionString(ConnStringKey_DefaultConnection);
            }
        }
        public string GoogleClientId
        {
            get
            {
                return config.GetAppSetting(AppSettingsKey_GoogleClientId);
            }
        }

        public string GoogleClientSecret
        {
            get
            {
                return config.GetAppSetting(AppSettingsKey_GoogleClientSecret);
            }
        }
    }
}
