using System.Collections.Generic;
using AutomationBase.Core.ObjectLayer.Pages;
using OpenQA.Selenium;

namespace AutomationBase.Core.Pages
{
    public class Login : BasePage
    {
        private Dictionary<string, By> locators = new Dictionary<string, By>()
        {
            { "username", By.Id("user-name") },
            { "password", By.Id("password") },
            { "login button", By.Id("login-button") }
        };

        public Login(IWebDriver driver): base(driver)
        {
            this.driver = driver;
        }

        public ProductsPage LoginToSite()
        {
            UIManager.FillText(locators["username"], TestEnvironment.username);
            UIManager.FillText(locators["password"], TestEnvironment.password);
            UIManager.Click(locators["login button"]);

            return new ProductsPage(driver);
        }
    }
}
