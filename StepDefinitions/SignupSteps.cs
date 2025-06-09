using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using System;
using System.IO;
using System.Threading.Tasks;
using YourProjectNamespace.Pages;

namespace YourProjectNamespace.Steps
{
    [Binding]
    public class SignupSteps
    {
        private IPage _page;
        private IBrowser _browser;
        private SignUpPage _signUpPage;
        private dynamic _testData;
        private string _generatedUsername;
        private string _generatedPassword;

        [BeforeScenario]
        public async Task Setup()
        {
            _testData = JObject.Parse(File.ReadAllText("TestData/signupdata.json"));

            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            _page = await _browser.NewPageAsync();
            _signUpPage = new SignUpPage(_page);
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }

        [Given(@"I navigate to the Demoblaze homepage")]
        public async Task GivenINavigateToTheDemoblazeHomepage()
        {
            await _page.GotoAsync((string)_testData.weburl);
        }

        [When(@"I click the Sign Up button")]
        public async Task WhenIClickTheSignUpButton()
        {
            await _signUpPage.ClickSignUpFromNavAsync();
        }

        [When(@"I fill in the sign-up form with random valid details")]
        public async Task WhenIFillInTheSignUpFormWithRandomValidDetails()
        {
            _generatedUsername = GenerateRandomString((string)_testData.usernamePrefix);
            _generatedPassword = GenerateRandomString((string)_testData.passwordPrefix);
            await _signUpPage.EnterSignUpDetailsAsync(_generatedUsername, _generatedPassword);
        }

        [When(@"I submit the sign-up form")]
        public async Task WhenISubmitTheSignUpForm()
        {
            await _signUpPage.SubmitSignUpAsync((string)_testData.signupSuccessMessage);
        }

        [Then(@"I should see a sign-up confirmation alert")]
        public void ThenIShouldSeeASignUpConfirmationAlert()
        {
            // Already handled in SubmitSignUpAsync using dialog assertion
            Assert.Pass("Sign up alert was successfully handled and verified.");
        }

        private string GenerateRandomString(string prefix)
        {
            return prefix + Guid.NewGuid().ToString("N").Substring(0, 6);
        }
    }
}
