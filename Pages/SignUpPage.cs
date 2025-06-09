using Microsoft.Playwright;
using System.Threading.Tasks;

namespace YourProjectNamespace.Pages
{
    public class SignUpPage
    {
        private readonly IPage _page;

        // Locators
        private readonly string signUpDashboardButton = "#signin2";
        private readonly string signUpUsernameInput = "#sign-username";
        private readonly string signUpPasswordInput = "#sign-password";
        private readonly string signUpButton = "button[onclick='register()']";
        private readonly string loginWelcomeText = "#nameofuser";

        public SignUpPage(IPage page)
        {
            _page = page;
        }

        public async Task ClickSignUpFromNavAsync()
        {
            await _page.ClickAsync(signUpDashboardButton);
            await _page.WaitForTimeoutAsync(1000); // wait for modal
        }

        public async Task EnterSignUpDetailsAsync(string username, string password)
        {
            await _page.FillAsync(signUpUsernameInput, username);
            await _page.FillAsync(signUpPasswordInput, password);
        }

        public async Task SubmitSignUpAsync(string expectedAlertMessage)
        {
            var tcs = new TaskCompletionSource<bool>();

            _page.Dialog += async (_, dialog) =>
            {
                if (dialog.Message == expectedAlertMessage)
                {
                    await dialog.AcceptAsync();
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(new Exception($"Unexpected alert message: {dialog.Message}"));
                }
            };

            await _page.ClickAsync(signUpButton);
            await tcs.Task;
        }

        public async Task<bool> IsUserLoggedIn(string username)
        {
            var actualText = await _page.InnerTextAsync(loginWelcomeText);
            return actualText.Contains(username);
        }
    }
}
