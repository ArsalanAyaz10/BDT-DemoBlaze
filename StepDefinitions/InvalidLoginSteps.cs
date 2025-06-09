using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using System.IO;
using System.Threading.Tasks;
using SqaProjectBDD.Helpers;  // <-- Added import for helper
using SqaProjectBDD.Pages;
using NUnit.Framework.Legacy;

namespace SqaProjectBDD.Steps
{
    [Binding]
    public class InvalidLoginSteps
    {
        private LoginPage _loginPage;
        private dynamic _testData;
        private string _alertMessage;

        [BeforeScenario]
        public async Task Setup()
        {
            _testData = JObject.Parse(File.ReadAllText("TestData/logindata.json"));

            // Initialize Playwright once and get the shared page instance
            await PlaywrightHelper.InitializeAsync();
            _loginPage = new LoginPage(PlaywrightHelper.Page);
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await PlaywrightHelper.DisposeAsync();
        }

        [Given(@"I open the Demoblaze website")]
        public async Task GivenIOpenTheDemoblazeWebsite()
        {
            string url = (string)_testData.url;
            await _loginPage.NavigateToHomePageAsync(url);
        }

        [When(@"I click on the login button")]
        public async Task WhenIClickOnTheLoginButton()
        {
            await _loginPage.ClickLoginButtonAsync();
        }

        [When(@"I enter invalid login credentials")]
        public async Task WhenIEnterInvalidLoginCredentials()
        {
            string username = (string)_testData.invalidLogin.username;
            string password = (string)_testData.invalidLogin.password;

            await _loginPage.EnterUsernameAsync(username);
            await _loginPage.EnterPasswordAsync(password);
        }

        [When(@"I submit the login form with invalid data")]
        public async Task WhenISubmitTheLoginFormWithInvalidData()
        {
            // Setup dialog handler before clicking submit
            PlaywrightHelper.Page.Dialog += async (_, dialog) =>
            {
                _alertMessage = dialog.Message;
                await dialog.AcceptAsync();
            };

            await _loginPage.SubmitLoginAsync();
            await PlaywrightHelper.Page.WaitForTimeoutAsync(2000); // buffer for dialog to appear
        }

        [Then(@"I should see an alert with the wrong password message")]
        public void ThenIShouldSeeAnAlertWithTheWrongPasswordMessage()
        {
            string expectedAlert = (string)_testData.invalidLogin.expectedAlert;
            ClassicAssert.AreEqual(expectedAlert, _alertMessage, "Alert message didn't match expected value.");
        }
    }
}
