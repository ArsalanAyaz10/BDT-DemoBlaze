Feature: Valid Login

  Scenario: Valid login with correct credentials
    Given I navigate to the website
    When I click the login button
    When I enter valid login credentials
    When I submit the login form
    Then I should see my username displayed

