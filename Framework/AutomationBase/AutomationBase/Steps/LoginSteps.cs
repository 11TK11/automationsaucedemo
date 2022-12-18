using AutomationBase.Core.Enum;
using AutomationBase.Core.Pages;
using AutomationBase.Helpers.UI;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomationBase.Steps
{
    [Binding]
    public sealed class LoginSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private ScenarioContext _scenarioContext;
        private readonly Login loginPage;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            var driver = (IWebDriver)_scenarioContext["driver"];
            loginPage = new Login(driver);
        }

        [Given(@"default user is logged")]
        public void GivenDefaultUserIsLogged()
        {
            var productPage = loginPage.LoginToSite();
            ScenarioContextHelper.Add(ref _scenarioContext, PageName.ProductsPage.ToString(), productPage);
        }
    }
}
