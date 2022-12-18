using System.Collections.Generic;
using System.Linq;

namespace AutomationBase.Helpers
{
    public class DictionaryHelper
    {
        public static string DictionaryToString(IDictionary<string, string> data)
        {
            return (data != null && data.Any()) ? string.Join("\n", data.Select(x => x.Key + ": " + x.Value)) : "<Dictionary is empty or null>.";
        }
    }
}
