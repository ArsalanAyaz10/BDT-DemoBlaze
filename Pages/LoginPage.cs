using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace SqaProjectBDD.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        // Selectors
        private string LoginButtonSelector => "#login2";
        private string UsernameInputSelector => "#loginusername";
        private string PasswordInputSelector => "#loginpassword";
    //    private string ModalSelector => "#logInModal";
    //  private string SubmitLoginButtonSelector => "button:has-text('Log in')";
        private string WelcomeUserSelector => "#nameofuser";

        public async Task NavigateToHomePageAsync(string url)
        {
            Console.WriteLine($"Navigating to: {url}");
            await _page.GotoAsync(url);
        }

        public async Task ClickLoginButtonAsync()
        {
            Console.WriteLine("Clicking top login button...");
            await _page.ClickAsync(LoginButtonSelector);
        }

        public async Task EnterUsernameAsync(string username)
        {
            Console.WriteLine($"Entering username: {username}");
            await _page.FillAsync(UsernameInputSelector, username);
        }

        public async Task EnterPasswordAsync(string password)
        {
            Console.WriteLine("Entering password...");
            await _page.FillAsync(PasswordInputSelector, password);
        }

        public async Task SubmitLoginAsync()
        {
            // Click login button and wait for navigation/reload if it happens
            await Task.WhenAll(
                _page.WaitForNavigationAsync(new() { WaitUntil = WaitUntilState.NetworkIdle }),
                _page.ClickAsync("button:has-text('Log in')")
            );

            // Now wait for username element visible after navigation
            await _page.Locator("#nameofuser").WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }


        public async Task<string> GetAlertTextAndAcceptAsync()
            {
                var dialogCompletion = new TaskCompletionSource<string>();

                _page.Dialog += async (_, dialog) =>
                {
                    Console.WriteLine($"Alert received: {dialog.Message}");
                    dialogCompletion.TrySetResult(dialog.Message);
                    await dialog.AcceptAsync();
                };

                var alertText = await dialogCompletion.Task;
                return alertText;
            }

            public async Task<bool> IsUserLoggedInAsync(string expectedUsername)
            {
                Console.WriteLine("Waiting for #nameofuser to appear...");
                try
                {
                    var nameLocator = _page.Locator(WelcomeUserSelector);
                    await nameLocator.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 10000 });

                    var actualText = await nameLocator.InnerTextAsync();
                    Console.WriteLine($"Login success text: {actualText}");
                    return actualText.Contains(expectedUsername);
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Timeout: #nameofuser did not appear.");
                    return false;
                }
            }
        }
    }
