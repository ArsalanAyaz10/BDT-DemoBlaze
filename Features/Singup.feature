Feature: User Sign Up

  As a new user
  I want to create an account on the demoblaze website
  So that I can log in and use the platform

  Scenario: Successful User Sign Up
    Given I navigate to the Demoblaze homepage
    When I click the Sign Up button
    And I fill in the sign-up form with random valid details
    And I submit the sign-up form
    Then I should see a sign-up confirmation alert
