using Microsoft.Playwright;
using NUnit.Framework;
using System.Runtime.Intrinsics.Arm;
using static Microsoft.Playwright.Assertions;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class LoginPlaywrightTests
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IPage _page = null!;

    [SetUp]
    public async Task SetUp()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 300
            }
        );

        _page = await _browser.NewPageAsync();

        await _page.SetContentAsync(
            """
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <title>Login Page</title>
            </head>

            <body>
                <h1>QA Automation Login</h1>

                <label for="username">Username</label>
               <input
                id="username"
                type="text"
                placeholder="Enter username">

                <br><br>

               <label for="password">Password</label>
               <input id="password" type="password">

               <br><br>

               <label for="remember-me">Remember me</label>
               <input id="remember-me" type="checkbox">
        
               <br><br>
                <label for="environment">Environment</label>

                <select id="environment">
                <option value="">Select environment</option>
                <option value="test">Test</option>
                <option value="staging">Staging</option>
                <option value="production">Production</option>
               </select>

            <br><br>
               <button id="login-button">Login</button>

               <p
                id="login-message"
                data-testid="login-message">
                </p>
            
            <br><br>

            <a href="#" id="forgot-password">
                Forgot password?
            </a>

            <p id="recovery-message"></p>

            <br><br>
            
                <button id="load-profile-button">
                    Load profile
                </button>
            
                <p
                    id="profile-message"
                    data-testid="profile-message"
                    style="display: none">
            </p>
            
            <br><br>

            <button id="save-settings-button" disabled>
                Save settings
            </button>

            <p
                id="settings-message"
                data-testid="settings-message">
            </p>
                        <br><br>

            <button id="prepare-dashboard-button">
                Prepare dashboard
            </button>

            <button
                id="open-dashboard-button"
                style="display: none">
                Open dashboard
            </button>

            <p
                id="dashboard-message"
                data-testid="dashboard-message">
            </p>
                <script>
                    const loginButton =
                        document.getElementById("login-button");

                    loginButton.addEventListener("click", () => {
                        const username =
                            document.getElementById("username").value;

                        const password =
                            document.getElementById("password").value;

                        const message =
                            document.getElementById("login-message");

                        if (
                            username === "pamela" &&
                            password === "qa123"
                        ) {
                            message.textContent =
                                "Login successful";
                        } else {
                            message.textContent =
                                "Invalid credentials";
                        }
                    });
                const forgotPasswordLink =
                document.getElementById("forgot-password");

                forgotPasswordLink.addEventListener("click", (event) => {
                  event.preventDefault();

                const recoveryMessage =
                    document.getElementById("recovery-message");

                recoveryMessage.textContent =
                    "Password recovery requested";

                });

                const loadProfileButton =
                document.getElementById("load-profile-button");

                loadProfileButton.addEventListener("click", () => {
                    const profileMessage =
                        document.getElementById("profile-message");

                    setTimeout(() => {
                        profileMessage.textContent =
                            "Profile loaded";

                        profileMessage.style.display =
                            "block";
                    }, 1500);
                  });

                              const environmentDropdown =
                document.getElementById("environment");

            const saveSettingsButton =
                document.getElementById("save-settings-button");

            const settingsMessage =
                document.getElementById("settings-message");

            environmentDropdown.addEventListener("change", () => {
                saveSettingsButton.disabled = true;

                setTimeout(() => {
                    saveSettingsButton.disabled =
                        environmentDropdown.value === "";
                }, 1000);
            });

            saveSettingsButton.addEventListener("click", () => {
                settingsMessage.textContent =
                    "Settings saved";
            });
                        const prepareDashboardButton =
                document.getElementById(
                    "prepare-dashboard-button"
                );

            const openDashboardButton =
                document.getElementById(
                    "open-dashboard-button"
                );

            const dashboardMessage =
                document.getElementById(
                    "dashboard-message"
                );

            prepareDashboardButton.addEventListener(
                "click",
                () => {
                    setTimeout(() => {
                        openDashboardButton.style.display =
                            "inline-block";
                    }, 1500);
                }
            );

            openDashboardButton.addEventListener(
                "click",
                () => {
                    dashboardMessage.textContent =
                        "Dashboard opened";
                }
            );
                </script>             
            </body>
            </html>
            """
        );
    }

    [Test]
    public async Task ValidCredentialsShouldDisplaySuccessMessage()
    {
        ILocator userNameInput =
            _page!.GetByLabel("Username");

        ILocator passwordInput =
            _page.GetByLabel("Password");

        ILocator loginButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Login" }
            );

        ILocator loginMessage =
            _page.Locator("#login-message");

        await userNameInput.FillAsync("pamela");
        await passwordInput.FillAsync("qa123");
        await loginButton.ClickAsync();
      
        await Expect(loginMessage)
                .ToHaveTextAsync("Login successful");
        
    }
   
    [Test]
    public async Task InvalidCredentialsShouldDisplayErrorMessage()
    {
        ILocator userNameInput =
            _page!.GetByLabel("Username");

        ILocator passwordInput =
            _page.GetByLabel("Password");

        ILocator loginButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Login" }
            );

        ILocator loginMessage =
            _page.Locator("#login-message");

        await userNameInput.FillAsync("pamela");
        await passwordInput.FillAsync("wrong-password");
        await loginButton.ClickAsync();

        string displayedMessage =
            await loginMessage.InnerTextAsync();

        Assert.That(
            displayedMessage,
            Is.EqualTo("Invalid credentials")
        );
    }

    [Test]
    public async Task RememberMeCheckboxShouldBeSelected()
    {
        ILocator rememberMeCheckbox =
            _page!.GetByLabel("Remember me");

        await rememberMeCheckbox.CheckAsync();

        await Expect(rememberMeCheckbox)
            .ToBeCheckedAsync();
    }

    [Test]
    public async Task ElementsShouldBeLocatedUsingDifferentStrategies()
    {
        ILocator heading =
            _page!.GetByRole(
                AriaRole.Heading,
                new() { Name = "QA Automation Login" }
            );

        ILocator userNameInput =
            _page.GetByPlaceholder("Enter username");

        ILocator passwordInput =
            _page.GetByLabel("Password");

        ILocator loginButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Login" }
            );

        ILocator loginMessage =
            _page.GetByTestId("login-message");

        string headingText =
            await heading.InnerTextAsync();

        await userNameInput.FillAsync("pamela");

        string enteredUserName =
            await userNameInput.InputValueAsync();

        await passwordInput.FillAsync("incorrect");
        await loginButton.ClickAsync();

        string displayedMessage =
            await loginMessage.InnerTextAsync();

        Assert.That(
            headingText,
            Is.EqualTo("QA Automation Login")
        );

        Assert.That(
            enteredUserName,
            Is.EqualTo("pamela")
        );

        Assert.That(
            displayedMessage,
            Is.EqualTo("Invalid credentials")
        );
    }

    [Test]
    public async Task EnvironmentShouldBeSelectedFromDropdown()
    {
        ILocator environmentDropdown =
            _page!.GetByLabel("Environment");

        await environmentDropdown.SelectOptionAsync("staging");

        await Expect(environmentDropdown)
            .ToHaveValueAsync("staging");
    }

    [Test]
    public async Task ForgotPasswordShouldDisplayRecoveryMessage()
    {
        ILocator forgotPasswordLink =
            _page!.GetByText("Forgot password?");

        await forgotPasswordLink.ClickAsync();

        ILocator recoveryMessage =
            _page.GetByText("Password recovery requested");

        string displayedMessage =
            await recoveryMessage.InnerTextAsync();

        Assert.That(
            displayedMessage,
            Is.EqualTo("Password recovery requested")
        );
    }

    [Test]
    public async Task ElementsShouldBeLocatedUsingCssAndXPath()
    {
        ILocator userNameInput =
            _page!.Locator("#username");

        ILocator loginButton =
            _page.Locator("button#login-button");

        ILocator forgotPasswordLink =
            _page.Locator("//a[@id='forgot-password']");

        await userNameInput.FillAsync("pamela");

        string enteredUserName =
            await userNameInput.InputValueAsync();

        string loginButtonText =
            await loginButton.InnerTextAsync();

        string forgotPasswordText =
            await forgotPasswordLink.InnerTextAsync();

        Assert.That(
            enteredUserName,
            Is.EqualTo("pamela")
        );

        Assert.That(
            loginButtonText,
            Is.EqualTo("Login")
        );

        Assert.That(
            forgotPasswordText.Trim(),
            Is.EqualTo("Forgot password?")
        );
    }

    [Test]
    public async Task ProfileMessageShouldAppearAfterLoading()
    {
        ILocator loadProfileButton =
            _page!.GetByRole(
                AriaRole.Button,
                new() { Name = "Load profile" }
            );

        ILocator profileMessage =
            _page.GetByTestId("profile-message");

        await Expect(profileMessage)
            .ToBeHiddenAsync();

        await loadProfileButton.ClickAsync();

        await Expect(profileMessage)
            .ToBeVisibleAsync(
                new() { Timeout = 3000 }
            );

        await Expect(profileMessage)
            .ToHaveTextAsync(
                "Profile loaded",
                new() { Timeout = 3000 }
            );
    }

    [Test]
    public async Task SaveSettingsButtonShouldBeEnabledAfterSelectingEnvironment()
    {
        ILocator environmentDropdown =
            _page!.GetByLabel("Environment");

        ILocator saveSettingsButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Save settings" }
            );

        ILocator settingsMessage =
            _page.GetByTestId("settings-message");

        await Expect(saveSettingsButton)
            .ToBeDisabledAsync();

        await environmentDropdown
            .SelectOptionAsync("test");

        await Expect(saveSettingsButton)
            .ToBeEnabledAsync();

        await saveSettingsButton.ClickAsync();

        await Expect(settingsMessage)
            .ToHaveTextAsync("Settings saved");
    }

    [Test]
    public async Task LoginPageShouldHaveExpectedStructure()
    {
        await Expect(_page!)
            .ToHaveTitleAsync("Login Page");

        ILocator heading =
            _page.GetByRole(
                AriaRole.Heading,
                new() { Name = "QA Automation Login" }
            );

        ILocator inputs =
            _page.Locator("input");

        ILocator passwordInput =
            _page.GetByLabel("Password");

        await Expect(heading)
            .ToBeVisibleAsync();

        await Expect(inputs)
            .ToHaveCountAsync(3);

        await Expect(passwordInput)
            .ToHaveAttributeAsync(
                "type",
                "password"
            );
    }

    [Test]
    public async Task SettingsMessageShouldContainSuccessText()
    {
        ILocator environmentDropdown =
            _page!.GetByLabel("Environment");

        ILocator saveSettingsButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Save settings" }
            );

        ILocator settingsMessage =
            _page.GetByTestId("settings-message");

        await environmentDropdown
            .SelectOptionAsync("production");

        await Expect(saveSettingsButton)
            .ToBeEnabledAsync();

        await saveSettingsButton.ClickAsync();

        await Expect(settingsMessage)
            .ToContainTextAsync("Settings");

        await Expect(settingsMessage)
            .ToContainTextAsync("saved");

        await Expect(settingsMessage)
            .Not
            .ToContainTextAsync("failed");
    }

    [Test]
    public async Task DashboardShouldOpenWhenButtonBecomesAvailable()
    {
        ILocator prepareDashboardButton =
            _page!.GetByRole(
                AriaRole.Button,
                new() { Name = "Prepare dashboard" }
            );

        ILocator openDashboardButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Open dashboard" }
            );

        ILocator dashboardMessage =
            _page.GetByTestId("dashboard-message");

        await prepareDashboardButton.ClickAsync();

        await openDashboardButton.ClickAsync();

        await Expect(dashboardMessage)
            .ToHaveTextAsync("Dashboard opened");
    }

    [Test]
    public async Task UserShouldCompleteLoginAndSettingsFlowSuccessfully()
    {
        ILocator userNameInput =
            _page.GetByLabel("Username");

        ILocator passwordInput =
            _page.GetByLabel("Password");

        ILocator rememberMeCheckbox =
            _page.GetByLabel("Remember me");

        ILocator environmentDropdown =
            _page.GetByLabel("Environment");

        ILocator loginButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Login" }
            );

        ILocator loginMessage =
            _page.GetByTestId("login-message");

        ILocator saveSettingsButton =
            _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Save settings" }
            );

        ILocator settingsMessage =
            _page.GetByTestId("settings-message");

        await Expect(_page)
            .ToHaveTitleAsync("Login Page");

        await Expect(userNameInput)
            .ToBeEditableAsync();

        await userNameInput.FillAsync("pamela");
        await passwordInput.FillAsync("qa123");
        await rememberMeCheckbox.CheckAsync();

        await environmentDropdown
            .SelectOptionAsync("production");

        await Expect(userNameInput)
            .ToHaveValueAsync("pamela");

        await Expect(passwordInput)
            .ToHaveValueAsync("qa123");

        await Expect(rememberMeCheckbox)
            .ToBeCheckedAsync();

        await Expect(environmentDropdown)
            .ToHaveValueAsync("production");

        await loginButton.ClickAsync();

        await Expect(loginMessage)
            .ToHaveTextAsync("Login successful");

        await Expect(saveSettingsButton)
            .ToBeEnabledAsync();

        await saveSettingsButton.ClickAsync();

        await Expect(settingsMessage)
            .ToHaveTextAsync("Settings saved");

        await Expect(settingsMessage)
            .Not
            .ToContainTextAsync("failed");
    }

    [Test]
    public async Task RecoveryAndProfileFlowShouldWorkCorrectly()
    {
        ILocator recoveryMessage =
           _page.GetByText("Password recovery requested");
     
        ILocator loadProfileButton =
           _page.GetByRole(
               AriaRole.Button,
               new() { Name = "Load profile" }
           );

        ILocator forgotPasswordLink =
           _page.GetByRole(
               AriaRole.Link,
               new() { Name = "Forgot password?" }
           );

        ILocator loadProfileMessage =
            _page.GetByTestId("profile-message");

        await Expect(loadProfileMessage)
            .ToBeHiddenAsync();

        await forgotPasswordLink.ClickAsync();

        await Expect(recoveryMessage)
    .ToHaveTextAsync("Password recovery requested");

        await Expect(recoveryMessage)
            .ToBeVisibleAsync();

        await loadProfileButton.ClickAsync();

        await Expect(loadProfileMessage)
            .ToBeVisibleAsync();

        await Expect(loadProfileMessage)
               .ToHaveTextAsync("Profile loaded");


        await Expect(loadProfileMessage)
            .Not
            .ToContainTextAsync("failed");


   }
    [Test]
    public async Task SettingsAndDashboardFlowShouldWorkCorrectly()
    {
        ILocator saveSettingsButton =
            _page.GetByRole(
                AriaRole.Button,
               new() { Name = "Save settings" }
               );

        ILocator prepareDashboardButton =
           _page.GetByRole(
               AriaRole.Button,
              new() { Name = "Prepare dashboard" }
              );

        ILocator openDashboardButton =
           _page.GetByRole(
               AriaRole.Button,
              new() { Name = "Open dashboard" }
              );


        await Expect(_page!)
          .ToHaveTitleAsync("Login Page");

        await Expect(saveSettingsButton)
            .ToBeDisabledAsync();

        ILocator environmentDropdown =
          _page!.GetByLabel("Environment");

        await environmentDropdown.SelectOptionAsync("staging");

        await Expect(environmentDropdown)
            .ToHaveValueAsync("staging");

        await Expect(saveSettingsButton)
            .ToBeEnabledAsync();

        await saveSettingsButton.ClickAsync();

        ILocator saveSettingsMessage = 
            _page!.GetByText("Settings saved");

        await Expect(saveSettingsMessage)
            .ToHaveTextAsync("Settings saved");

        await Expect(saveSettingsMessage)
            .Not
            .ToContainTextAsync("error");

        await prepareDashboardButton.ClickAsync();
        await openDashboardButton.ClickAsync();

        ILocator dashboardOpenedMessage =
            _page!.GetByText("Dashboard opened");

        await Expect(dashboardOpenedMessage)
            .ToHaveTextAsync("Dashboard opened");
       }



    [TearDown]
    public async Task TearDown()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}