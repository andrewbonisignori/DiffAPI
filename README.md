# Diff API
Diff API to compare two blocks of data and evaluate the differences between them.

The Diff API is available online at endpoint http://diff.azurewebsites.net/

The Swagger documentation and UI to make some tests are available at http://diff.azurewebsites.net/swagger

In order to execute a Diff analisys, are necessary two calls to the endpoints in order to provide the left and right data to be diff-ed, as shown below.

### First Call - API request to add left data to id 1.

    POST /v1/diff/1/left
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

### Second Call - API request to add right data to id 1.

    POST /v1/diff/1/right
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTJfNDU2X19f"

### Sample API request to retrieve analisys for id 1.

    GET /v1/diff/1
    Host: diff.azurewebsites.net

### Sample result from above analysis.

    {
        "Status": "DifferencesFound",
        "DiffBlocks": [
            {
                "Offset": 3,
                "Lenght": 1
            },
            {
                "Offset": 7,
                "Lenght": 3
            }
        ]
    }
