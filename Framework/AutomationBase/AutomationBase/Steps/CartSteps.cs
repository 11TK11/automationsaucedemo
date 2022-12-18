using System.Collections.Generic;
using AutomationBase.Core.Enum;
using AutomationBase.Core.Pages;
using AutomationBase.Helpers.UI;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AutomationBase.Steps
{
    [Binding]
    public sealed class CartSteps
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private ScenarioContext _scenarioContext;
        private CartPage cartPage;

        public CartSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            cartPage = ScenarioContextHelper.Get<CartPage>(_scenarioContext, PageName.CartPage.ToString());
        }

        [Then(@"on 'Cart' page, the following products are displayed")]
        public void ThenOnPageTheFollowingProductsAreDisplayed(Table table)
        {
            IEnumerable<dynamic> dataTable = table.CreateDynamicSet();
            List<string> mismatchingValues = cartPage.GetMismatchingDisplayedProducts(dataTable);
            Assert.IsTrue(mismatchingValues.Count == 0, $"Mismatching values for:\n {string.Join("\n", mismatchingValues)}");
        }
    }
}
