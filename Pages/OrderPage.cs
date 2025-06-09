using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SqaProjectBDD.Pages
{
    public class OrderPage
    {
        private readonly IPage _page;

        public OrderPage(IPage page)
        {
            _page = page;
        }

        // Navigate to cart
        public async Task NavigateToCartAsync()
        {
            await _page.ClickAsync("//a[@id='cartur']");
        }

        // Click on "Place Order" button
        public async Task ClickPlaceOrderAsync()
        {
            await _page.ClickAsync("//button[normalize-space()='Place Order']");
        }

        // Fill order form with provided data
        public async Task FillOrderFormAsync(PurchaseData data)
        {
            await _page.FillAsync("#name", data.Name);
            await _page.FillAsync("#country", data.Country);
            await _page.FillAsync("#card", data.CreditCard);
            await _page.FillAsync("#month", data.Month);
            await _page.FillAsync("#year", data.Year);
        }

        // Click "Purchase" to confirm the order
        public async Task ClickPurchaseAsync()
        {
            await _page.ClickAsync("//button[normalize-space()='Purchase']");
        }

        // Check if confirmation message appears after successful purchase
        public async Task<bool> IsOrderConfirmedAsync()
        {
            var confirmation = await _page.WaitForSelectorAsync(
                "//h2[contains(text(),'Thank you for your purchase!')]",
                new PageWaitForSelectorOptions { Timeout = 5000 }
            );
            return confirmation != null;
        }

        public async Task<bool> IsOrderModalStillVisibleAsync()
        {
            var modal = await _page.QuerySelectorAsync("#orderModal");
            if (modal == null) return false;
            return await modal.IsVisibleAsync();
        }
        public async Task<bool> IsOrderConfirmationVisibleAsync()
        {
            var confirmation = await _page.QuerySelectorAsync("//h2[contains(text(),'Thank you for your purchase!')]");
            return confirmation != null && await confirmation.IsVisibleAsync();
        }

    }
    public class PurchaseData
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string CreditCard { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }
}
