Feature: HTTP testing

    Scenario: basic
        Given the base url http://localhost:5000
        And the http headers
            | name | value |
            | foo  | 123   |
        # And an error is expected
        When I invoke POST /echo?x=1
            """
            {
                "data": "value"
            }
            """
        Then compare response.IsSuccessStatusCode = true
        And query result matches
            """
            method = "POST"
            and path = "/echo"
            and $lookup(headers, "foo") = ["123"]
            and query[key="x"].value
            """