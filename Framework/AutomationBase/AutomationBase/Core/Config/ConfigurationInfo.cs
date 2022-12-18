using Microsoft.Extensions.Configuration;

namespace AutomationBase.Core.Config
{
    public class ConfigurationInfo
    {
        public AppSettings appSettings { get; }
        public Environment environment { get; }
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
            environment = sectionProjectConfiguration.Get<Environment>();
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
