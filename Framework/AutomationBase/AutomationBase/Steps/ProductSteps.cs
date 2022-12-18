using System.Collections.Generic;
using AutomationBase.Core.Enum;
using AutomationBase.Core.Pages;
using AutomationBase.Helpers;
using AutomationBase.Helpers.UI;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AutomationBase.Steps
{
    [Binding]
    public sealed class ProductSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private ScenarioContext _scenarioContext;
        private ProductsPage productsPage;

        public ProductSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            productsPage = ScenarioContextHelper.Get<ProductsPage>(_scenarioContext, PageName.ProductsPage.ToString());
        }

        [When(@"on 'Products' page, set order to '(.*)'")]
        public void WhenOnPageSetOrderTo(string orderType)
        {
            productsPage.SetProductOrder(orderType);
        }

        [Then(@"on 'Products' page, current products are sorted by '(.*)'")]
        public void ThenOnPageCurrentProductsAreSortedBy(string orderType)
        {
            Dictionary<string, string> mismatchingValues = productsPage.GetMismatchingOrderedProducts(orderType);
            Assert.IsTrue(mismatchingValues.Count == 0, $"Mismatching values for:\n {DictionaryHelper.DictionaryToString(mismatchingValues)}");
        }

        [When(@"on 'Products' page, click on 'ADD TO CART' button for the following prodcuts")]
        public void WhenOnPageClickOnButtonForTheFollowingProdcuts(Table table)
        {
            IEnumerable<dynamic> dataTable = table.CreateDynamicSet();
            productsPage.AddMultipleItemsToCart(dataTable);
        }

        [Then(@"on 'Products' page, button 'ADD TO CART' changed text to '(.*)' for the following products")]
        public void ThenOnPageButtonChangedTextTo(string newName, Table table)
        {
            IEnumerable<dynamic> dataTable = table.CreateDynamicSet();
            List<string> mismatchingValues = productsPage.GetMismatchingButtonNames(newName, dataTable);
            Assert.IsTrue(mismatchingValues.Count == 0, $"Mismatching values for:\n {string.Join("\n", mismatchingValues)}");
        }

        [When(@"on 'Products' page, click on 'Cart' icon")]
        public void WhenOnPageClickOnIcon()
        {
            var cartPage = productsPage.ClickCartIcon();
            ScenarioContextHelper.Add(ref _scenarioContext, PageName.CartPage.ToString(), cartPage);
        }
    }
}
