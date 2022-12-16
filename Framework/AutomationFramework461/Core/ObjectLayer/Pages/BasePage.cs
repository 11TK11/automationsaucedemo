using AutomationFramework461.Helpers.UI;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework461.Core.ObjectLayer.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver driver;
        protected UIManager UIManager;
        private Dictionary<string, By> baseLocators = new Dictionary<string, By>()
        {
            { "Ellipsis", By.XPath("//div[@class='lds-ellipsis']")},
            { "Title", By.TagName("h3") }
        };

        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
            UIManager = new UIManager(driver);
        }

        public abstract string GetExpectedUrl(string company, string sectionName);

        public void RefreshPage()
        {
            UIManager.RefreshPage();
        }

        public void WaitSpinnerDisappear()
        {
            UIManager.WaitElementDisappear(baseLocators["Ellipsis"]);
        }

        public string GetCurrentUrl()
        {
            return UIManager.GetdirectURL();
        }

        public string GetPageTitle()
        {
            return UIManager.GetText(baseLocators["Title"]);
        }

        public List<string> AreLabelsDisplayed(string[] data)
        {
            List<string> validationResult = new List<string>();
            foreach (var item in data)
            {
                if (!UIManager.IsElementDisplayed(By.XPath($"//label[contains(text(), '{item}')]")))
                {
                    validationResult.Add($"Label {item} is not present");
                }
            }

            return validationResult;
        }
    }
}
