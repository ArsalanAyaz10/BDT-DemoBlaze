using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Reqnroll;
using SqaProjectBDD.Pages;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YourProjectNamespace.Steps
{
    [Binding]
    public class CheckoutSteps
    {
        private IPage _page;
        private IBrowser _browser;
        private OrderPage _orderPage;
        private dynamic _testData;

        [BeforeScenario]
        public async Task Setup()
        {
            // Read test data from JSON
            _testData = JObject.Parse(File.ReadAllText("TestData/purchaseData.json"));

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

        [Given(@"nagivate Demoblaze homepage")]
        public async Task GivenIOpenTheDemoblazeHomepage()
        {
            string url = _testData.weburl?.ToString();
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("weburl is not defined in the JSON test data.");

            await _page.GotoAsync(url);
        }

        [When(@"add ""(.*)"" product to the cart")]
        public async Task WhenIAddProductToTheCart(string productName)
        {
            await _page.ClickAsync($"//a[normalize-space()='{productName}']");
            await _page.ClickAsync("//a[@class='btn btn-success btn-lg']");

            // Handle alert for 'Product added'
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _page.WaitForTimeoutAsync(2000);

            // Navigate back to home (using logo link or nav ID)
            await _page.ClickAsync("//a[@id='nava']");
        }

        [When(@"Navigate to cart")]
        public async Task WhenIGoToTheCart()
        {
            await _orderPage.NavigateToCartAsync();
        }

        [When(@"I click on place order")]
        public async Task WhenIClickOnPlaceOrder()
        {
            await _orderPage.ClickPlaceOrderAsync();
        }

        [When(@"I fill purchase form with valid data")]
        public async Task WhenIFillPurchaseFormWithValidData()
        {
            var data = new PurchaseData
            {
                Name = _testData.name?.ToString(),
                Country = _testData.country?.ToString(),
                CreditCard = _testData.creditCard?.ToString(),
                Month = _testData.month?.ToString(),
                Year = _testData.year?.ToString()
            };

            await _orderPage.FillOrderFormAsync(data);
        }

        [When(@"I confirm the purchase")]
        public async Task WhenIConfirmThePurchase()
        {
            await _orderPage.ClickPurchaseAsync();
        }

        [Then(@"I should see a purchase confirmation")]
        public async Task ThenIShouldSeeAPurchaseConfirmation()
        {
            var result = await _orderPage.IsOrderConfirmedAsync();
            ClassicAssert.IsTrue(result, "Purchase confirmation not found.");
        }
    }
}
