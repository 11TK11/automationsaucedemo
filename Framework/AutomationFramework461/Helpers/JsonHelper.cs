using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace AutomationFramework461.Helpers
{
    public class JsonHelper
    {
        public static T GetFileSection<T>(string sectionName, string jsonFile)
        {
            jsonFile = $"{AppDomain.CurrentDomain.BaseDirectory}{jsonFile}";
            IConfiguration config = new ConfigurationBuilder()
                                    .AddJsonFile(jsonFile)
                                    .Build();
            var section = config.GetSection(sectionName);
            return section.Get<T>();
        }
    }
}
