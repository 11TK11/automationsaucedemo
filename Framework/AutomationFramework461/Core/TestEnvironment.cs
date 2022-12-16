using AutomationFramework461.Core.Config;

namespace AutomationFramework461.Core
{
    public class TestEnvironment
    {
        public static string browser
        {
            get
            {
                return ConfigurationInfo.Instance.appSettings.Browser;
            }
        }

        public static bool IsDevEnvironment()
        {
            return ConfigurationInfo.Instance.appSettings.Site.ToLower() == "dev";
        }

        public static bool IsQaEnvironment()
        {
            return ConfigurationInfo.Instance.appSettings.Site.ToLower() == "qa";
        }
    }
}
