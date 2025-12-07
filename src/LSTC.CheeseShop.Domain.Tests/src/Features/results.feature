@Unit
Feature: Generic Results
  In order to provide a generic testing framework
  As a developer
  I want to set and test results

  # Scenario: JSONata query
  #   Then json { "items": true } query 'items' is true

  # Scenario: JSONata query with substitution
  #   Given the value X is 123
  #   Then json { "items": $$X } query 'items' is 123

  # Scenario: JSONata query with full substitution
  #   Given the value X is { "items": 123 }
  #   Then json $$X query 'items' is 123

  Scenario Outline: Setting values
    Given the value <name> is <value>
    Then compare <name> = <value>

    Examples:
      | name | value            |
      | A    | true             |
      | B    | false            |
      | C    | 1                |
      | D    | -1               |
      | E    | 0.0              |
      | F    | "string"         |
      | G    | { "foo": "bar" } |
      | H    | []               |

  Scenario Outline: Settings values as body
    Given the value X is
      """
      {
        "items": [
          1,
          2,
          3
        ]
      }
      """
    Then compare X =
      """
      {
        "items": [
          1,
          2,
          3
        ]
      }
      """

  Scenario: Value substitutions
    Given the value X is "X"
    And the value Y is "${X}Y"
    And the value Z is "${Y}Z"
    Then compare Z = "XYZ"

  Scenario: Enter values as tables
    Given the values
      | name | value            |
      | A    | true             |
      | B    | false            |
      | C    | 1                |
      | D    | -1               |
      | E    | 0.0              |
      | F    | "string"         |
      | G    | { "foo": "bar" } |
      | H    | []               |
    Then the values
      | lhs | op | rhs              |
      | A   | =  | true             |
      | B   | =  | false            |
      | C   | =  | 1                |
      | D   | =  | -1               |
      | E   | =  | 0.0              |
      | F   | =  | "string"         |
      | G   | =  | { "foo": "bar" } |
      | H   | =  | []               |

  Scenario: Jsonata query
    Given the value DATA is
      """
      {
        "items": [
          1,
          2,
          3
        ]
      }
      """
    Then query DATA matches '$count(items) > 1'