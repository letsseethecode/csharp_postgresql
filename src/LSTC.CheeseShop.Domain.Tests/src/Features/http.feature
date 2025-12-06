Feature: HTTP testing

    Scenario: basic
        Given the base url http://localhost:5000
        And the http headers
            | name | value |
            | foo  | 123   |
        # And an error is expected
        When I invoke POST /echo
            """
            {
                "data": "value"
            }
            """
        Then the result.IsSuccessStatusCode = true
