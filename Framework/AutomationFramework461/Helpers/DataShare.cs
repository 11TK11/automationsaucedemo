using System.Collections.Generic;

namespace AutomationFramework461.Helpers
{
    public class DataShare
    {
        private static DataShare instance = null;
        public IDictionary<string, object> Data = new Dictionary<string, object>();

        private DataShare()
        {
        }

        public static DataShare Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataShare();
                }
                return instance;
            }
        }

        public void Add(string key, object value)
        {
            if (Data.ContainsKey(key)) {
                Data[key] = value;
            }
            else
            {
                Data.Add(key, value);
            }
        }
    }
}
