Feature: Contact Message Functionality
  As a user of Demoblaze
  I want to send a contact message
  So that I can reach out to the company

  Scenario: Send a valid contact message
    Given I open the Demoblaze website for contact
    When I click the contact button
    And I fill in the contact form with valid data
    And I send the contact message
    Then I should see a confirmation alert
