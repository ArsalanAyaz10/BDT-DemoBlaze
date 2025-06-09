Feature: User Login
  As a user of Demoblaze
  I want to verify that incorrect login credentials show an appropriate error

  Scenario: Invalid Login with wrong credentials
    Given I am on the Demoblaze homepage
    When I click the Log in button
    And I enter invalid login credentials
    And I submit the login form
    Then I should see an alert saying "demoblaze.com says\nWrong password."
