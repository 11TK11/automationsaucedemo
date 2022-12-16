using AutomationFramework461.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace AutomationFramework461.Helpers.UI
{
    public class UIManager
    {
        private IWebDriver driver;
        private const Double DEFAULT_TIMEOUT = 60;

        public UIManager(IWebDriver driver)
        {
            this.driver = driver;
        }

        #region Browser actions
        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }

        public void NavigateToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public int GetRowsCount(By table) => driver.FindElements(table).Count();

        public string GetdirectURL()
        {
            return driver.Url;
        }
        #endregion

        #region Navigate
        public void NavigateInMenu(By menuItem, By itemToMove)
        {
            MoveMouse(menuItem);
            IWebElement webElement = driver.FindElement(menuItem);
            webElement.Click();
            if (TestEnvironment.IsDevEnvironment())
            {
                ForceWait(50);
            }
            MoveMouse(itemToMove);
        }

        public IDictionary<int, IDictionary<string, string>> GetDicOfDicFromWebElements(By by, string headerXpath)
        {
            IDictionary<int, IDictionary<string, string>> actualTable = new Dictionary<int, IDictionary<string, string>>();

            IList<IWebElement> webElements = driver.FindElements(by);
            for (int row = 0; row < webElements.Count; row++)
            {
                IList<IWebElement> getValueByColumns = webElements[row].FindElements(By.XPath("mat-cell"));

                IDictionary<string, string> valuesInRow = new Dictionary<string, string>();
                for (int column = 0; column < getValueByColumns.Count; column++)
                {
                    string nameHeader = driver.FindElement(By.XPath(string.Format(headerXpath, column + 1))).Text;
                    valuesInRow.Add(nameHeader, getValueByColumns[column].Text);
                }

                actualTable.Add(row, valuesInRow);
            }

            return actualTable;
        }
        #endregion

        #region Dropdown
        public void ChooseValueOptionInDropDownFromPlaceholderContainMatch(By by, string value)
        {
            MoveMouseAndClick(by);
            By optionValue = By.XPath($"//mat-option/span[contains(text(), '{value}')]");
            ClickElementUsingActions(optionValue);
        }

        public void CloseDropdown(By by)
        {
            IWebElement webElement = driver.FindElement(by);
            webElement.SendKeys(Keys.Escape);
        }
        #endregion

        #region Click
        public void Click(By by)
        {
            WaitElementIsPresent(by);
            IWebElement webElement = driver.FindElement(by);
            new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT)).Until(ExpectedConditions.ElementToBeClickable(by)).Click();
        }

        public void ClickElementUsingActions(By by)
        {
            IWebElement element = driver.FindElement(by);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element).Click().Build().Perform();
        }

        public void MoveMouseAndClick(By by, double timeout = DEFAULT_TIMEOUT)
        {
            WaitElementIsPresent(by);
            IWebElement webElement = driver.FindElement(by);
            PointOnElement(webElement);
            WaitElementIsVisible(by);
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementToBeClickable(by)).Click();
        }
        #endregion

        #region Move Mouse
        public void MoveMouse(By by)
        {
            WaitElementIsPresent(by);
            IWebElement webElement = driver.FindElement(by);
            PointOnElement(webElement);
        }

        public void ScrollUpToTop()
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scroll(0,-1000);");
        }

        public void PointOnElement(IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }
        #endregion

        #region Keyboard Actions
        public void PressEnterKey()
        {
            Actions builder = new Actions(driver);
            builder = builder.SendKeys(Keys.Enter);
            IAction multiple = builder.Build();
            multiple.Perform();
        }
        #endregion

        #region Waits
        public void ForceWait(int waitSeconds = 2)
        {
            Thread.Sleep(waitSeconds * 1000);
        }

        public void WaitElementIsPresent(By by, Double timeout = DEFAULT_TIMEOUT)
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
        
        public void WaitElementDisappear(By locator, Double timeout = DEFAULT_TIMEOUT)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (Exception)
            {
                throw new Exception($"\nTimed out after {timeout} seconds.\nElement still present: {locator}");
            }
        }

        public void WaitElementIsClickable(By by, Double timeout = DEFAULT_TIMEOUT)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementToBeClickable(by));
        }

        public void WaitElementIsVisible(By by, Double timeout = DEFAULT_TIMEOUT)
        {

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception)
            {
                throw new Exception($"\nTimed out after {timeout} seconds.\nElement not visible: {by}");
            }
        }
        #endregion

        #region Fill Text
        public void FillText(By by, string value)
        {
            IWebElement element = driver.FindElement(by);
            WaitElementIsClickable(by);
            element.Click();
            element.Clear();
            if (value != "<Empty>")
            {
                element.SendKeys(value);
            }
            WaitElementIsClickable(by, 2);
        }

        public void ClearTextFieldWithValidation(By by)
        {
            IWebElement webElement = driver.FindElement(by);
            webElement.Clear();
            webElement.SendKeys("clear@red.txt");
            for (int i = 0; i < 13; i++)
            {
                webElement.SendKeys(Keys.Backspace);
            }
        }

        public void ClearCommonTextField(By by)
        {
            IWebElement webElement = driver.FindElement(by);
            webElement.Clear();
        }

        public void EnterDataIntoTextFieldWithValidation(By by, string value)
        {
            IWebElement webElement = driver.FindElement(by);
            foreach (var character in value)
            {
                webElement.SendKeys(character.ToString());
            }
            webElement.SendKeys(Keys.Space);
            webElement.SendKeys(Keys.Backspace);
        }

        public void FillText(string label, string value)
        {
            By by = By.XPath($"//label[text()='{label}']/following-sibling::input");
            IWebElement element = driver.FindElement(by);
            WaitElementIsClickable(by);
            element.Click();
            element.Clear();
            element.SendKeys(value);
            WaitElementIsClickable(By.XPath($"//label[text()='{label}']/following-sibling::input"), 2);
        }

        public void SetCheckbox(string labelField, string value)
        {
            By checkBoxLocator = By.XPath($"//label[text()='{labelField}']/following-sibling::input");
            IWebElement checkbox = driver.FindElement(checkBoxLocator);

            if (bool.Parse(value) && !checkbox.Selected)
            {
                Click(checkBoxLocator);
            }
            else if (!bool.Parse(value) && checkbox.Selected)
            {
                Click(checkBoxLocator);
            }
        }
        public void FillTextPlusbutton(string label, string value)
        {
            IWebElement element = driver.FindElement(By.XPath($"//label[text()='{label}']/following-sibling::div/input"));
            element.Click();
            element.Clear();
            element.SendKeys(value);
        }

        public void FillTextPassword(string label, string value)
        {
            By by = By.XPath($"//label[text()='{label}']/following-sibling::div//input");
            IWebElement element = driver.FindElement(by);
            WaitElementIsClickable(by);
            IWebElement iconEye = driver.FindElement(By.XPath("//span[@class = 'icon-eye']"));
            iconEye.Click();
            element.Click();
            element.Clear();
            element.SendKeys(value);
        }
        public void ChooseValueOptionInDropDown(string label, string value)
        {
            IWebElement dropdown = driver.FindElement(By.XPath($"//label[text()='{label}']/following-sibling::select"));
            SelectElement select = new SelectElement(dropdown);
            select.SelectByText(value);
        }

        public void ChooseValueOptionInDropDown(By by, string value)
        {
            if (value != "<Empty>")
            {
                IWebElement dropdown = driver.FindElement(by);
                dropdown.Click();
                IWebElement option = dropdown.FindElement(By.XPath($"//option[text() = '{value}']"));
                option.Click();
            }
        }

        public void ChooseValueOptionInDropDownCheckBox(string label, string value)
        {
            IWebElement dropdown = driver.FindElement(By.XPath($"//mat-select[@aria-label = '{label}']"));
            dropdown.Click();
            ReadOnlyCollection<IWebElement> dropdownOptions = dropdown.FindElements(By.XPath("//*[contains(@id, 'mat-option')]"));

            // Unselect all options
            foreach (IWebElement option in dropdownOptions)
            {
                option.Click();
            }

            IWebElement optionToSelect = driver.FindElement(By.XPath($"//*[contains(@id, 'mat-option')]//*[contains(text(), '{value}')]"));
            optionToSelect.Click();
            dropdown.SendKeys(Keys.Escape);
        }

        public void ChooseValueOptionInDropDownOmsnic(string label, string value)
        {
            IWebElement dropdown = driver.FindElement(By.XPath($"//omsnic-select[@placeholder='{label}']/descendant::select"));
            SelectElement select = new SelectElement(dropdown);
            select.SelectByText(value);
        }

        public void ChooseValueOptionInDropDownFromPlaceholder(string label, string value)
        {
            By dropdownPlaceHolder = By.XPath($"//mat-select[@placeholder='{label}']");
            MoveMouseAndClick(dropdownPlaceHolder);

            By optionValue = By.XPath($"//span[text() = '{value}']");
            MoveMouseAndClick(optionValue);
        }

        public void ChooseValueOptionInDropdownQuickQuote(string label, string value)
        {
            By dropdownLocator = By.XPath($"//p[contains(text(),'{label}')]//mat-select");
            MoveMouseAndClick(dropdownLocator);

            By optionValue = By.XPath($"//mat-option//span[contains(text(),'{value}')]");
            MoveMouseAndClick(optionValue);

            driver.FindElement(dropdownLocator).SendKeys(Keys.Escape);
        }

        public void FillTextWithPlaceHolder(string label, string value)
        {
            By by = By.XPath($"//input[@placeholder = '{label}']");
            IWebElement element = driver.FindElement(by);
            WaitElementIsClickable(by);
            element.Click();
            element.Clear();
            element.SendKeys(value);
        }

        public void FillTextFieldQuickQuote(string label, string value)
        {
            By by = By.XPath($"//p[contains(text(),'{label}')]/descendant::input");
            IWebElement element = driver.FindElement(by);
            MoveMouseAndClick(by);
            element.Clear();
            element.SendKeys(value);
            //WaitQuickQuoteSpinnerDisappear();
        }

        public void FillTextIntoAnimation(string label, string value)
        {
            By by = By.XPath($"//mat-label[contains(text(),'{label}')]/ancestor::div/input");
            IWebElement element = driver.FindElement(by);
            MoveMouseAndClick(by);
            element.Clear();
            element.SendKeys(value);
            //WaitQuickQuoteSpinnerDisappear();
        }

        public void SetSwitchCheckBox(string label, string value)
        {
            IWebElement checkbox = driver.FindElement(By.XPath($"//omsnic-switch[@text = '{label}']/descendant::input"));
            IWebElement switchButton = driver.FindElement(By.XPath($"//omsnic-switch[@text = '{label}']/descendant::span[@class='switch-switch']"));
            if (value.Equals("YES") && !checkbox.Selected)
            {
                switchButton.Click();
            }
            else
            {
                if (value.Equals("NO") && checkbox.Selected)
                    switchButton.Click();
            }
        }
        #endregion

        #region Get Properties
        public string GetText(By by)
        {
            IWebElement element = driver.FindElement(by);
            return element.Text;
        }

        public string GetElementAttribute(By locator, string attribute)
        {
            IDictionary<string, string> propertiesAccordingElementType = new Dictionary<string, string>()
            {
                { "textnoclearwaitrefresh","max" },
                { "checkbox", "aria-checked" },
                { "textdisabled", "ng-reflect-value" },
                { "dropdowntotextdisabled", "ng-reflect-value" },
                { "dropdownintoplaceholdercontainmatch", "innerText" },
                { "celltext", "innerText" }
            };
            attribute = propertiesAccordingElementType.ContainsKey(attribute) ? propertiesAccordingElementType[attribute] : attribute;
            IWebElement element = driver.FindElement(locator);
            return element.GetAttribute(attribute);
        }
        #endregion

        #region Property verification
        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsElementDisplayed(By by)
        {
            bool isElementDisplayed = false;
            try
            {
                isElementDisplayed = driver.FindElement(by).Displayed;
            }
            catch (Exception)
            {
                isElementDisplayed = false;
            }

            return isElementDisplayed;
        }

        public bool IsElementEnabled(By locator)
        {
            return driver.FindElement(locator).Enabled;
        }

        public string[] GetTextArrayFromWebElements(By by)
        {
            IList<IWebElement> webElements = driver.FindElements(by);
            List<string> texts = new List<string>();

            foreach (var webElement in webElements)
            {
                texts.Add(webElement.GetAttribute("innerText"));
            }
            return texts.ToArray();
        }
        #endregion

        #region Table Actions
        public IList<IWebElement> GetListOfWebElements(By by)
        {
            WaitElementIsPresent(by);
            return driver.FindElements(by);
        }

        public string GetRowDataFromTableCommon(By locator, string value, By pagination)
        {
            string rowText = string.Empty;
            bool isNextEnabled = false;
            do
            {
                IList<IWebElement> rows = driver.FindElements(locator);
                foreach (IWebElement row in rows)
                {
                    if (row.Text.Contains(value))
                    {
                        rowText = row.Text;
                        break;
                    }
                }
                isNextEnabled = IsElementEnabled(pagination);
                if (isNextEnabled && rowText == string.Empty)
                {
                    Click(pagination);
                    ForceWait(5);
                }
            } while (isNextEnabled && rowText == string.Empty);

            return rowText;
        }

        public By GetRowButtonClassLocator(By locator, string value, By pagination, string classButton = "icon-edit-2")
        {
            By rowEditButtton = null;
            bool isNextEnabled = false;
            int rowNumber = 1;
            do
            {
                IList<IWebElement> rows = driver.FindElements(locator);
                foreach (IWebElement row in rows)
                {
                    if (row.Text.Contains(value))
                    {
                        rowEditButtton = By.XPath($"(//span[@class='{classButton}']/parent::button)[{rowNumber}]");
                        break;
                    }
                    rowNumber++;
                }
                isNextEnabled = IsElementEnabled(pagination);
                if (isNextEnabled && rowEditButtton == null)
                {
                    Click(pagination);
                    ScrollUpToTop();
                    rowNumber = 1;
                    ForceWait(5000);
                }
            } while (isNextEnabled && rowEditButtton == null);

            return rowEditButtton;
        }

        public IDictionary<string, string> GetRowDataFromTableCommon(By locator, By header, By cellLocator, string value, By pagination)
        {
            IDictionary<string, string> rowElements = new Dictionary<string, string>();
            IList<string> headerTitles = driver.FindElements(header).Select(element => element.Text).ToList();
            bool isNextEnabled = false;
            do
            {
                IList<IWebElement> rows = driver.FindElements(locator);
                int rowPosition = 0;
                foreach (IWebElement row in rows)
                {
                    if (row.Text.Contains(value))
                    {
                        IList<IWebElement> cells = driver.FindElements(cellLocator);
                        for (int i = 0; i < headerTitles.Count - 1; i++)
                        {
                            rowElements.Add(headerTitles[i], cells[(headerTitles.Count * rowPosition) + i].Text);
                        }
                        break;
                    }
                    rowPosition++;
                }
                isNextEnabled = IsElementEnabled(pagination);
                if (isNextEnabled && rowElements.Count == 0)
                {
                    Click(pagination);
                    ForceWait(5);
                }
            } while (isNextEnabled && rowElements.Count == 0);

            return rowElements;
        }
        #endregion

        #region Populate
        public void Populate(string labelField, string value, string type)
        {
            By by;
            switch (type)
            {
                case "text":
                    FillText(labelField, value);
                    break;
                case "checkbox":
                    SetCheckbox(labelField, value);
                    break;
                case "textPlus":
                    FillTextPlusbutton(labelField, value);
                    break;
                case "textPassword":
                    FillTextPassword(labelField, value);
                    break;
                case "dropdown":
                    ChooseValueOptionInDropDown(labelField, value);
                    break;
                case "dropdownTraining":
                    by = By.Id("id_training");
                    ChooseValueOptionInDropDown(by, value);
                    break;
                case "dropdownCheckbox":
                    ChooseValueOptionInDropDownCheckBox(labelField, value);
                    break;
                case "dropdownOmsnic":
                    ChooseValueOptionInDropDownOmsnic(labelField, value);
                    break;
                case "dropdownIntoPlaceholder":
                    ChooseValueOptionInDropDownFromPlaceholder(labelField, value);
                    break;
                case "dropdownQuickQuote":
                    ChooseValueOptionInDropdownQuickQuote(labelField, value);
                    break;
                case "textPlaceHolder":
                    FillTextWithPlaceHolder(labelField, value);
                    break;
                case "textFieldQuickQuote":
                    FillTextFieldQuickQuote(labelField, value);
                    break;
                case "textIntoAnimation":
                    FillTextIntoAnimation(labelField, value);
                    break;
                case "switchCheckBox":
                    SetSwitchCheckBox(labelField, value);
                    break;
                default:
                    throw new Exception("Type not supported: " + type);
            }
        }
        #endregion
    }
}
