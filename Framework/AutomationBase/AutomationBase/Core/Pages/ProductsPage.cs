using System.Collections.Generic;
using AutomationBase.Core.ObjectLayer.Pages;
using OpenQA.Selenium;

namespace AutomationBase.Core.Pages
{
    public class ProductsPage : BasePage
    {
        private Dictionary<string, By> locators = new Dictionary<string, By>()
        {
            { "Order Selector", By.ClassName("product_sort_container") },
            { "Product Title", By.ClassName("inventory_item_name") },
            { "Cart Icon", By.ClassName("shopping_cart_link") }
        };

        public ProductsPage(IWebDriver driver) : base(driver)
        {
            this.driver = driver;
        }

        public void SetProductOrder(string orderType)
        {
            UIManager.ChooseValueOptionInDropDown(locators["Order Selector"], orderType);
        }

        public Dictionary<string, string> GetMismatchingOrderedProducts(string orderType)
        {
            Dictionary<string, string> mismatchedItems = new Dictionary<string, string>();
            List<string> ascendingOrders = new List<string>() { "NAME (A TO Z)" };
            List<string> currentTitles = UIManager.GetTextFromWebElements(locators["Product Title"]);
            List<string> sortedTitles = new List<string>(currentTitles);

            sortedTitles.Sort();
            if (!ascendingOrders.Contains(orderType.ToUpper()))
            {
                sortedTitles.Reverse();
            }
            
            for (int i = 0; i < currentTitles.Count; i++)
            {
                if (currentTitles[i] != sortedTitles[i])
                {
                    mismatchedItems.Add($"Expected: {sortedTitles[i]}", $"Actual: {currentTitles[i]}");
                }
            }

            return mismatchedItems;
        }

        public void AddMultipleItemsToCart(IEnumerable<dynamic> dataTable)
        {
            foreach (var item in dataTable)
            {
                string buttonId = $"add-to-cart-{item.Product.ToLower().Replace(" ", "-")}";
                UIManager.Click(By.Id(buttonId));
            }
        }

        public List<string> GetMismatchingButtonNames(string newName, IEnumerable<dynamic> dataTable)
        {
            List<string> mismatchedItems = new List<string>();

            foreach (var item in dataTable)
            {
                string buttonId = $"remove-{item.Product.ToLower().Replace(" ", "-")}";
                if (UIManager.GetText(By.Id(buttonId)) != newName)
                {
                    mismatchedItems.Add($"Product {item.Product} didn't change its text to {newName}");
                }
            }

            return mismatchedItems;
        }

        public CartPage ClickCartIcon()
        {
            UIManager.Click(locators["Cart Icon"]);
            return new CartPage(driver);
        }
    }
}
