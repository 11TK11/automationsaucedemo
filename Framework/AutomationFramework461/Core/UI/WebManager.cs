using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;

namespace AutomationFramework461.Core.UI
{
    public class WebManager
    {
        private IWebDriver driver;
        private Dictionary<string, Func<IDriverConfig>> configDrivers = new Dictionary<string, Func<IDriverConfig>>()
        {
            { "chrome", () => new ChromeConfig() }
        };
        private Dictionary<string, Func<IWebDriver>> browsers = new Dictionary<string, Func<IWebDriver>>
        {
            { "chrome", () => new ChromeDriver() }
        };

        public WebManager()
        {
            //this.ScenarioContext = ScenarioContext;
            new DriverManager().SetUpDriver(configDrivers[TestEnvironment.browser]());
            driver = browsers[TestEnvironment.browser]();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(40);
        }

        public IWebDriver GetWebDriver()
        {
            return driver;
        }

        public void QuitDriver()
        {
            driver.Quit();
        }
    }
}
