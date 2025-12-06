@Unit
Feature: Generic Results
  In order to provide a generic testing framework
  As a developer
  I want to set and test results

  Scenario: JSONata query
    Then json { "items": true } query 'items' is true

  Scenario: JSONata query with substitution
    Given the value X is 123
    Then json { "items": $$X } query 'items' is 123

  Scenario: JSONata query with full substitution
    Given the value X is { "items": 123 }
    Then json $$X query 'items' is 123

  Scenario Outline: Setting default results
    Given the result X is <value>
    Then the result X = <value>
    Examples:
      | value            |
      | true             |
      | false            |
      | 1                |
      | -1               |
      | 0.0              |
      | "string"         |
      | { "foo": "bar" } |
      | []               |

  Scenario Outline: Setting named results
    Given the result <name> is <value>
    Then the result <name> = <value>
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

  Scenario Outline: Setting values
    Given the value <name> is <value>
    Then the value <name> = <value>

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

  Scenario: Enter values as tables
    Given the values:
      | name | value            |
      | A    | true             |
      | B    | false            |
      | C    | 1                |
      | D    | -1               |
      | E    | 0.0              |
      | F    | "string"         |
      | G    | { "foo": "bar" } |
      | H    | []               |
    Then the values are:
      | name | op | value            |
      | A    | =  | true             |
      | B    | =  | false            |
      | C    | =  | 1                |
      | D    | =  | -1               |
      | E    | =  | 0.0              |
      | F    | =  | "string"         |
      | G    | =  | { "foo": "bar" } |
      | H    | =  | []               |

  Scenario Outline: Comparison operators
    Given the value A is <a>
    And the value B is <b>
    Then value A <op> B

    Examples:
      | a | op | b |
      | 1 | <  | 2 |
      | 1 | <= | 2 |
      | 3 | =  | 3 |
      | 4 | >= | 3 |
      | 5 | >  | 4 |

  Scenario Outline: Direct comparison operators
    Then value <a> <op> <b>
    Examples:
      | a | op | b |
      | 1 | <  | 2 |
      | 1 | <= | 2 |
      | 3 | =  | 3 |
      | 4 | >= | 3 |
      | 5 | >  | 4 |

# Scenario Outline: Jsonata matches
#   Given the value DATA is <data>
#   And the value QUERY is <query>
#   Then json $$DATA query QUERY
#   Examples:
#     | data                 | query               |
#     | { "lines": [1,2,3] } | "$count(lines) > 0" |

# Scenario Outline: Jsonata direct matches
#   Then jsonata <data> matches <query>
#   Examples:
#     | data                 | query               |
#     | { "lines": [1,2,3] } | "$count(lines) > 0" |

# Scenario Outline: Jsonata query
#   Given the value DATA is <data>
#   And the value QUERY is <query>
#   And the value RESULT is <result>
#   Then value DATA query QUERY is RESULT
#   Examples:
#     | data                 | query               | result |
#     | { "lines": [1,2,3] } | "$count(lines) > 0" | true   |

# Scenario Outline: Jsonata direct query
#   Given the value DATA is <data>
#   And the value QUERY is <query>
#   And the value RESULT is <result>
#   Then value { "lines": [1,2,3] } query '$count(lines) > 0' is true
#   Examples:
#     | data                 | query               | result |
#     | { "lines": [1,2,3] } | "$count(lines) > 0" | true   |
