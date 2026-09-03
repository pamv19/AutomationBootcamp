using Microsoft.Playwright;
using NUnit.Framework;
using Reqnroll;

namespace AutomationBootcamp.Tests.StepDefinitions;

[Binding]
public class WebNavigationStepDefinitions
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage? _page;

    [Given("the browser is open")]
    public async Task GivenTheBrowserIsOpen()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });

        _page = await _browser.NewPageAsync();
    }

    [When("a local test page is loaded")]
    public async Task WhenALocalTestPageIsLoaded()
    {
        await _page!.SetContentAsync("""
        <html>
            <head>
                <title>Automation Bootcamp</title>
            </head>
            <body>
                <h1>My first Playwright test</h1>
            </body>
        </html>
        """);
    }

    [Then("the page title should be {string}")]
    public async Task ThenThePageTitleShouldBe(string expectedTitle)
    {
        string actualTitle = await _page!.TitleAsync();

        Assert.That(actualTitle, Is.EqualTo(expectedTitle));
    }

    [AfterScenario]
    public async Task CloseBrowser()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}