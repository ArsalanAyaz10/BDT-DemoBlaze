Feature: Invalid Checkout

  Scenario: Attempt to place order without filling in any form details
    Given I am on the Demoblaze homepage
    When I add "Nokia lumia 1520" to the cart
    And I navigate to the cart
    And I click on Place Order
    And I submit the purchase form without entering any data
    Then I should not see a purchase confirmation
