using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SqaProjectBDD.Pages;
using SqaProjectBDD.Helpers;
using NUnit.Framework.Legacy;

namespace SqaProjectBDD.Steps
{
    [Binding]
    public class AboutUsSteps
    {
        private AboutUsPage _aboutUsPage;
        private dynamic _testData;

        [BeforeScenario]
        public async Task Setup()
        {
            _testData = JObject.Parse(File.ReadAllText("TestData/aboutusdata.json"));
            await PlaywrightHelper.InitializeAsync();
            _aboutUsPage = new AboutUsPage(PlaywrightHelper.Page);
        }

        [AfterScenario]
        public async Task TearDown()
        {
            await PlaywrightHelper.DisposeAsync();
        }

        [Given("I go to the Demoblaze website")]
        public async Task GivenIOpenTheDemoblazeWebsite()
        {
            string url = (string)_testData.url;
            await PlaywrightHelper.Page.GotoAsync(url);
        }

        [When("I open the About Us modal")]
        public async Task WhenIOpenTheAboutUsModal()
        {
            await _aboutUsPage.OpenAboutUsModalAsync();
        }

        [When("I close the About Us modal")]
        public async Task WhenICloseTheAboutUsModal()
        {
            await _aboutUsPage.CloseAboutUsModalAsync();
        }

        [Then("the About Us modal should be closed")]
        public async Task ThenTheAboutUsModalShouldBeClosed()
        {
            var modalVisible = await PlaywrightHelper.Page.Locator("#videoModal").IsVisibleAsync();
            ClassicAssert.IsFalse(modalVisible, "The About Us modal should be closed but it's still visible.");
        }
    }
}
