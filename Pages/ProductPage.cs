using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SqaProjectBDD.Pages
{
    public class ProductPage
    {
        private readonly IPage _page;

        public ProductPage(IPage page)
        {
            _page = page;
        }

        // Selectors
        private string ProductCardSelector => "//div[@class='col-lg-9']//div[2]//div[1]//a[1]//img[1]";
        private string ProductHeadingSelector => "//a[normalize-space()='Nokia lumia 1520']";
        private string AddToCartButtonSelector => "//a[@class='btn btn-success btn-lg']";

        // Click product card image to go to product details
        public async Task ClickOnProductCardAsync()
        {
            await _page.Locator(ProductCardSelector).ClickAsync();
        }

        // Alternatively, click on product heading link
        public async Task ClickOnProductHeadingAsync()
        {
            await _page.Locator(ProductHeadingSelector).ClickAsync();
        }

        // Click on Add to Cart button on product details page
        public async Task ClickAddToCartAsync()
        {
            await _page.Locator(AddToCartButtonSelector).ClickAsync();
        }
    }
}
