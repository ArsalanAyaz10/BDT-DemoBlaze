using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SqaProjectBDD.Pages
{
    public class DeleteItemPage
    {
        private readonly IPage _page;

        public DeleteItemPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToCartAsync()
        {
            await _page.ClickAsync("//a[normalize-space()='Cart']");
        }

        public async Task DeleteFirstItemAsync()
        {
            await _page.ClickAsync("//a[normalize-space()='Delete']");
        }

        public async Task<bool> IsCartEmptyAsync()
        {
            // Wait briefly to allow cart to update
            await _page.WaitForTimeoutAsync(2000);
            var rows = await _page.QuerySelectorAllAsync("//tr");
            // If only the table header is left, the item was deleted
            return rows.Count <= 1;
        }
    }
}
