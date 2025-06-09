using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SqaProjectBDD.Pages
{
    public class ContactPage
    {
        private readonly IPage _page;

        public ContactPage(IPage page)
        {
            _page = page;
        }

        // Selectors
        private string ContactEmailSelector => "#recipient-email";
        private string ContactNameSelector => "#recipient-name";
        private string ContactMessageSelector => "#message-text";
        private string SendMessageButtonSelector => "xpath=//div[@id='exampleModal']//button[text()='Send message']";

        public async Task OpenContactModalAsync()
        {  var contactButton = _page.Locator("//a[normalize-space()='Contact']");
            await contactButton.ClickAsync();

            // Wait for the contact modal to be visible
            await _page.Locator("#exampleModal").WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }

        public async Task FillContactFormAsync(string email, string name, string message)
        {
            await _page.FillAsync(ContactEmailSelector, email);
            await _page.FillAsync(ContactNameSelector, name);
            await _page.FillAsync(ContactMessageSelector, message);
        }

        public async Task SubmitContactFormAsync()
        {
            // Handle alert dialog
            var dialogTask = new TaskCompletionSource<string>();
            _page.Dialog += async (_, dialog) =>
            {
                dialogTask.SetResult(dialog.Message);
                await dialog.AcceptAsync();
            };

            await _page.ClickAsync(SendMessageButtonSelector);
            await dialogTask.Task; // Wait for alert to appear
        }
    }
}
