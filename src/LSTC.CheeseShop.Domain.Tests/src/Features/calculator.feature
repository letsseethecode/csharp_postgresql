Feature: Calculator
  In order to avoid mistakes
  As a math user
  I want to be able to add two numbers

  @Unit
  Scenario: Add two numbers
    Given I have a calculator
    When I add 5 and 3
    Then the result should be 8
