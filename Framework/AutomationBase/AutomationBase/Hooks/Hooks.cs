using BoDi;
using AutomationBase.Core.UI;
using TechTalk.SpecFlow;

namespace AutomationBase.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private WebManager webManager;
        private readonly IObjectContainer objectContainer;

        public Hooks(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void Before(ScenarioContext scenarioContext)
        {
            webManager = new WebManager();
            var driver = webManager.GetWebDriver();
            scenarioContext["driver"] = driver;
            objectContainer.RegisterInstanceAs(driver);
        }

        [AfterScenario]
        public void AfterFeature()
        {
            webManager.QuitDriver();
        }
    }
}
