using Microsoft.Extensions.Configuration;

namespace AutomationFramework461.Core.Config
{
    public class ConfigurationInfo
    {
        public AppSettings appSettings { get; }
        private static ConfigurationInfo instance;
        private ConfigurationInfo()
        {
            IConfiguration config = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();
            var sectionAppSettings = config.GetSection(nameof(AppSettings));
            appSettings = sectionAppSettings.Get<AppSettings>();
            var sectionProjectConfiguration = config.GetSection("ProjectInfo")
                                                    .GetSection(appSettings.Site.ToLower());
        }

        public static ConfigurationInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConfigurationInfo();
                }

                return instance;
            }
        }
    }
}
