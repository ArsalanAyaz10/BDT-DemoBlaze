using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SqaProjectBDD.Helpers
{
    public class PlaywrightHelper
    {
        private static IPlaywright _playwright;
        private static IBrowser _browser;
        private static IBrowserContext _context;
        private static IPage _page;

        public static async Task InitializeAsync()
        {
            if (_playwright == null)
            {
                _playwright = await Playwright.CreateAsync();
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
                _context = await _browser.NewContextAsync();
                _page = await _context.NewPageAsync();
            }
        }

        public static IPage Page => _page;

        public static async Task DisposeAsync()
        {
            if (_browser != null)
            {
                await _browser.CloseAsync();
                _browser = null;
                _playwright = null;
                _context = null;
                _page = null;
            }
        }
    }
}
