using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using Reqnroll;
using SqaProjectBDD.Helpers;
using SqaProjectBDD.Pages;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqaProjectBDD.Steps
{
    [Binding]
    public class ContactSteps
    {
        private ContactPage _contactPage;
        private dynamic _testData;
        private string _alertMessage;

        [BeforeScenario]
        public async Task Setup()
        {
            _testData = JObject.Parse(File.ReadAllText("TestData/contactdata.json"));

            await PlaywrightHelper.InitializeAsync();
            _contactPage = new ContactPage(PlaywrightHelper.Page);
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await PlaywrightHelper.DisposeAsync();
        }

        [Given("I open the Demoblaze website for contact")]
        public async Task GivenIOpenTheDemoblazeWebsite()
        {
            string url = (string)_testData.url;
            await PlaywrightHelper.Page.GotoAsync(url);
        }

        [When("I click the contact button")]
        public async Task WhenIClickTheContactButton()
        {
            await _contactPage.OpenContactModalAsync();
        }

        [When("I fill in the contact form with valid data")]
        public async Task WhenIFillInTheContactFormWithValidData()
        {
            string email = (string)_testData.contact.email;
            string name = (string)_testData.contact.name;
            string message = (string)_testData.contact.message;

            await _contactPage.FillContactFormAsync(email, name, message);
        }

        [When("I send the contact message")]
        public async Task WhenISendTheContactMessage()
        {
            // Capture the alert text
            var dialogTask = new TaskCompletionSource<string>();
            PlaywrightHelper.Page.Dialog += async (_, dialog) =>
            {
                _alertMessage = dialog.Message;
                await dialog.AcceptAsync();
                dialogTask.SetResult(dialog.Message);
            };

            await _contactPage.SubmitContactFormAsync();
            await dialogTask.Task; // wait for alert
        }

        [Then("I should see a confirmation alert")]
        public void ThenIShouldSeeAConfirmationAlert()
        {
            Assert.That(_alertMessage, Is.Not.Null.And.Not.Empty, "Alert message was not received.");
        }
    }
}
