Feature: Add item to cart

  Scenario: Add Nokia Lumia 1520 to the shopping cart
    Given open Demoblaze website
    When I click on the Nokia Lumia 1520 product
    And I add the product to the cart
    Then I should see a confirmation alert for adding the product
