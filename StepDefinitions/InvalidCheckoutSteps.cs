using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using SqaProjectBDD.Pages;
using NUnit.Framework.Legacy;

namespace YourProjectNamespace.Steps
{
    [Binding]
    public class InvalidCheckoutSteps
    {
        private IPage _page;
        private IBrowser _browser;
        private OrderPage _orderPage;

        [BeforeScenario]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();

            _orderPage = new OrderPage(_page);
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }

        [Given(@"I am on the Demoblaze homepage")]
        public async Task GivenIAmOnTheDemoblazeHomepage()
        {
            await _page.GotoAsync("https://www.demoblaze.com");  // or use test data URL here
        }

        [When(@"I add ""(.*)"" to the cart")]
        public async Task WhenIAddProductToTheCart(string productName)
        {
            await _page.ClickAsync($"//a[normalize-space()='{productName}']");
            await _page.ClickAsync("//a[@class='btn btn-success btn-lg']");

            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.WaitForTimeoutAsync(2000);

            await _page.ClickAsync("//a[@id='nava']");
        }

        [When(@"I navigate to the cart")]
        public async Task WhenINavigateToTheCart()
        {
            await _orderPage.NavigateToCartAsync();
        }

        [When(@"I click on Place Order")]
        public async Task WhenIClickOnPlaceOrder()
        {
            await _orderPage.ClickPlaceOrderAsync();
        }

        [When(@"I submit the purchase form without entering any data")]
        public async Task WhenISubmitThePurchaseFormWithoutEnteringAnyData()
        {
            // Note: Do NOT fill any form fields here.
            await _orderPage.ClickPurchaseAsync();
        }

        [Then(@"I should not see a purchase confirmation")]
        public async Task ThenIShouldNotSeeAPurchaseConfirmation()
        {
            // Use a quick check to verify the confirmation modal does NOT appear
            var isConfirmed = await _orderPage.IsOrderConfirmationVisibleAsync();
            ClassicAssert.IsFalse(isConfirmed, "Purchase confirmation should NOT be visible when submitting empty form.");
        }
    }
}
