using Microsoft.Playwright;
using Reqnroll;
using SqaProjectBDD.Pages;

namespace SqaProjectBDD.Steps
{
    [Binding]
    public class DeleteItemSteps
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IPage _page;
        private DeleteItemPage _deleteItemPage;

        [Given("Open the Demoblaze website")]
        public async Task GivenIOpenTheDemoblazeWebsite()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();

            _deleteItemPage = new DeleteItemPage(_page);

            // Handle alert automatically
            _page.Dialog += async (_, dialog) => await dialog.AcceptAsync();

            await _page.GotoAsync("https://www.demoblaze.com");

            // Add item to cart
            await _page.ClickAsync("//a[normalize-space()='Nokia lumia 1520']");
            await _page.ClickAsync("//a[@class='btn btn-success btn-lg']");

            // Optional wait after alert
            await _page.WaitForTimeoutAsync(2000);

            // Go back to home page
            await _page.ClickAsync("a.navbar-brand");
        }


        [When("I go to the cart")]
        public async Task WhenIGoToTheCart()
        {
            await _deleteItemPage.NavigateToCartAsync();
        }

        [When("I delete an item from the cart")]
        public async Task WhenIDeleteAnItemFromTheCart()
        {
            await _deleteItemPage.DeleteFirstItemAsync();
        }

        [Then("the item should be removed from the cart")]
        public async Task ThenTheItemShouldBeRemovedFromTheCart()
        {
            var isDeleted = await _deleteItemPage.IsCartEmptyAsync();
            if (!isDeleted)
                throw new System.Exception("The cart still contains items.");
        }

    }
}
