Feature: About Us Modal

  Scenario: Open and close the About Us modal
    Given I go to the Demoblaze website
    When I open the About Us modal
    And I close the About Us modal
    Then the About Us modal should be closed
