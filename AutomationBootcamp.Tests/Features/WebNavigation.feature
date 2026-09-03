Feature: Web navigation

    Scenario: Load a local test page
        Given the browser is open
        When a local test page is loaded
        Then the page title should be "Automation Bootcamp"