using TechTalk.SpecFlow;

namespace AutomationFramework461.Helpers.UI
{
    public class ScenarioContextHelper
    {
        public static T Get<T>(ScenarioContext scenarioContext, string objectName)
        {
            return (T)scenarioContext[objectName];
        }

        public static void Add(ref ScenarioContext _scenarioContext, string objectName, object objectToAdd)
        {
            if (_scenarioContext.ContainsKey(objectName))
            {
                _scenarioContext[objectName] = objectToAdd;
            }
            else
            {
                _scenarioContext.Add(objectName, objectToAdd);
            }
        }
    }
}
