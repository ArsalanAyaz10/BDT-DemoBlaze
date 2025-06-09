using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using Reqnroll;
using SqaProjectBDD.Pages;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SqaProjectBDD.Helpers;

namespace SqaProjectBDD.Steps
{
    [Binding]
    public class ValidLoginSteps
    {
        private LoginPage _loginPage;
        private dynamic _testData;

        public ValidLoginSteps()
        {
            // Empty constructor; initialization happens in Setup
        }

        [BeforeScenario]
        public async Task Setup()
        {
            _testData = JObject.Parse(File.ReadAllText("TestData/logindata.json"));

            await PlaywrightHelper.InitializeAsync();
            _loginPage = new LoginPage(PlaywrightHelper.Page);
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await PlaywrightHelper.DisposeAsync();
        }

        [Given(@"I navigate to the website")]
        public async Task GivenINavigateToTheWebsite()
        {
            string url = (string)_testData.url;
            await _loginPage.NavigateToHomePageAsync(url);
        }

        [When("I click the login button")]
        public async Task WhenIClickTheLoginButton()
        {
            await _loginPage.ClickLoginButtonAsync();
        }

        [When(@"I enter valid login credentials")]
        public async Task WhenIEnterValidLoginCredentials()
        {
            string username = (string)_testData.validLogin.username;
            string password = (string)_testData.validLogin.password;

            await _loginPage.EnterUsernameAsync(username);
            await _loginPage.EnterPasswordAsync(password);
        }

        [When("I submit the login form")]
        public async Task WhenISubmitTheLoginForm()
        {
            await _loginPage.SubmitLoginAsync();
        }

        [Then(@"I should see my username displayed")]
        public async Task ThenIShouldSeeMyUsernameDisplayed()
        {
            string expectedText = (string)_testData.validLogin.username;
            var userElement = await PlaywrightHelper.Page.WaitForSelectorAsync("#nameofuser", new() { Timeout = 50000, State = WaitForSelectorState.Visible });
            string actualText = await userElement.InnerTextAsync();

            Assert.That(actualText.Trim(), Is.EqualTo($"Welcome {expectedText}"), "Username welcome text mismatch");
        }
    }
}
