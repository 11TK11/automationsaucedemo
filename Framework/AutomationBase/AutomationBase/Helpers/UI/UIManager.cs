using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace AutomationBase.Helpers.UI
{
    public class UIManager
    {
        private IWebDriver driver;
        private const double DefaultTimeout = 60;

        public UIManager(IWebDriver driver)
        {
            this.driver = driver;
        }

        #region Dropdown

        public void ChooseValueOptionInDropDown(By by, string value)
        {
            IWebElement dropdown = driver.FindElement(by);
            SelectElement select = new SelectElement(dropdown);
            select.SelectByText(value);
        }
        #endregion

        #region Click
        public void Click(By by)
        {
            WaitElementIsPresent(by);
            IWebElement webElement = driver.FindElement(by);
            new WebDriverWait(driver, TimeSpan.FromSeconds(DefaultTimeout)).Until(ExpectedConditions.ElementToBeClickable(by)).Click();
        }
        #endregion

        #region Waits

        public void WaitElementIsPresent(By by, double timeout = DefaultTimeout)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementExists(by));
            }
            catch (Exception)
            {
                throw new Exception($"\nTimed out after {timeout} seconds.\nElement not present: {by}");
            }
        }
        #endregion

        #region Fill Text
        public void FillText(By by, string value)
        {
            IWebElement element = driver.FindElement(by);
            element.SendKeys(value);
        }
        #endregion

        #region Get Properties
        public string GetText(By by)
        {
            IWebElement element = driver.FindElement(by);
            return element.Text;
        }
        #endregion

        #region Property verification

        public List<string> GetTextFromWebElements(By by)
        {
            IList<IWebElement> webElements = driver.FindElements(by);
            List<string> texts = new List<string>();

            foreach (var webElement in webElements)
            {
                texts.Add(webElement.GetAttribute("innerText"));
            }
            return texts;
        }
        #endregion
    }
}
