using System.Collections.Generic;
using AutomationBase.Core.ObjectLayer.Pages;
using OpenQA.Selenium;

namespace AutomationBase.Core.Pages
{
    public class CartPage : BasePage
    {
        private Dictionary<string, By> locators = new Dictionary<string, By>()
        {
            { "Item Names", By.ClassName("inventory_item_name") }
        };

        public CartPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        public List<string> GetMismatchingDisplayedProducts(IEnumerable<dynamic> dataTable)
        {
            List<string> mismatchingValues = new List<string>();
            List<string> currentItems = UIManager.GetTextFromWebElements(locators["Item Names"]);
            foreach (var item in dataTable)
            {
                if (!currentItems.Contains(item.Product))
                {
                    mismatchingValues.Add($"Iem {item.Product} not in list");
                }
            }

            return mismatchingValues;
        }
    }
}
