using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SqaProjectBDD.Pages
{
    public class AboutUsPage
    {
        private readonly IPage _page;

        public AboutUsPage(IPage page)
        {
            _page = page;
        }

        private string AboutUsButtonSelector => "//a[normalize-space()='About us']";
        private string CloseButtonSelector => "//div[@id='videoModal']//button[text()='Close']";

        public async Task OpenAboutUsModalAsync()
        {
            var aboutUsButton = _page.Locator(AboutUsButtonSelector);
            await aboutUsButton.ClickAsync();

            // Wait for the modal to appear
            await _page.Locator("#videoModal").WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }

        public async Task CloseAboutUsModalAsync()
        {
            var closeButton = _page.Locator(CloseButtonSelector);
            await closeButton.ClickAsync();

            // Wait for the modal to disappear
            await _page.Locator("#videoModal").WaitForAsync(new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });
        }
    }
}
