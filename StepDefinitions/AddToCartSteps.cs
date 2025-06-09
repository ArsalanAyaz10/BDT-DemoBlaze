
using Microsoft.Playwright;
using NUnit.Framework.Legacy;
using SqaProjectBDD.Helpers;
using SqaProjectBDD.Pages;


namespace SqaProjectBDD.Steps
{
    [Binding]
    public class AddToCartSteps
    {
        private readonly IPage _page;
        private ProductPage _productPage;
        private string _alertMessage;

        public AddToCartSteps()
        {
            _page = PlaywrightHelper.Page;  // Replace with your Playwright page instance provider
            _productPage = new ProductPage(_page);
        }

        [Given("open Demoblaze website")]
        public async Task GivenIOpenTheDemoblazeWebsite()
        {
            await _page.GotoAsync("https://www.demoblaze.com");
        }

        [When("I click on the Nokia Lumia 1520 product")]
        public async Task WhenIClickOnTheNokiaLumia1520Product()
        {
            // You can choose to click on either card image or heading - here clicking heading
            await _productPage.ClickOnProductHeadingAsync();

            // Wait for product detail page to load
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        [When("I add the product to the cart")]
        public async Task WhenIAddTheProductToTheCart()
        {
            // Handle the alert popup after adding product to cart
            var tcs = new TaskCompletionSource<string>();
            _page.Dialog += async (_, dialog) =>
            {
                _alertMessage = dialog.Message;
                await dialog.AcceptAsync();
                tcs.SetResult(_alertMessage);
            };

            await _productPage.ClickAddToCartAsync();

            // Wait for the alert dialog to appear and be accepted
            await tcs.Task;
        }

        [Then("I should see a confirmation alert for adding the product")]
        public void ThenIShouldSeeAConfirmationAlertForAddingTheProduct()
        {
            ClassicAssert.IsNotNull(_alertMessage, "Alert message was not captured");
            ClassicAssert.IsTrue(_alertMessage.Contains("Product added"), $"Unexpected alert message: {_alertMessage}");
        }
    }
}
