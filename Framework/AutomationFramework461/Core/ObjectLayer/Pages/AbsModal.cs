using AutomationFramework461.Helpers.UI;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework461.Core.ObjectLayer.Pages
{
    public abstract class AbsModal
    {
        protected UIManager UIManager;
        protected IWebDriver driver;
        protected Dictionary<string, By> locators = new Dictionary<string, By>()
        {
            { "Close", By.Id("closeModal") },
            { "Modal Body", By.CssSelector(".modal-content") }
        };

        protected AbsModal(IWebDriver driver)
        {
            this.driver = driver;
            UIManager = new UIManager(driver);
        }

        public void CloseModal()
        {
            UIManager.Click(locators["Close"]);
        }

        public void WaitModalDisappear()
        {
            UIManager.WaitElementDisappear(locators["Modal Body"]);
        }
    }
}
