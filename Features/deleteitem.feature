Feature: Remove item from cart
  As a user
  I want to remove an item from the cart
  So that I can manage my cart items

  Scenario: Delete an item from the cart
    Given Open the Demoblaze website
    When I go to the cart
    And I delete an item from the cart
    Then the item should be removed from the cart
