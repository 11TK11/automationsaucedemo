using AutomationBase.Helpers.UI;
using OpenQA.Selenium;

namespace AutomationBase.Core.ObjectLayer.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected UIManager UIManager;

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
            UIManager = new UIManager(driver);
        }
    }
}
