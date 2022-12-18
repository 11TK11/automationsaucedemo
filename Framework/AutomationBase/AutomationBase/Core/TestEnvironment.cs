using AutomationBase.Core.Config;

namespace AutomationBase.Core
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

        public static string url
        {
            get
            {
                return ConfigurationInfo.Instance.environment.Url;
            }
        }

        public static string username
        {
            get
            {
                return ConfigurationInfo.Instance.environment.Username;
            }
        }

        public static string password
        {
            get
            {
                return ConfigurationInfo.Instance.environment.Password;
            }
        }
    }
}
