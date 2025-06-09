Feature: Place Order Functionality
  As a user of the Demoblaze website
  I want to add a product to the cart and complete the checkout
  So that I can purchase the item successfully

  Scenario: Successfully placing an order
    Given nagivate Demoblaze homepage
    When  add "Nokia lumia 1520" product to the cart
    And Navigate to cart
    And I click on place order
    And I fill purchase form with valid data
    And I confirm the purchase
    Then I should see a purchase confirmation
