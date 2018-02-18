# Diff API
Diff API to compare two blocks of data and evaluate the differences between them.

The Diff API is available online at endpoint http://diff.azurewebsites.net/

The documentation and UI to execute some tests are available at http://diff.azurewebsites.net/swagger

Two blocks of data(base 64 string) are expected in order to a analysis be executed. These blocks are divided in left side and right side. Each of these blocks are provide to API using a different call and using the same {id} to associate them, as shown below.

### First Call - Update left data using {id} 1
Data being sent: 123456789

    POST /v1/diff/1/left
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

### Second Call - Update right data using {id} 1
Data being sent: 12#456###

    POST /v1/diff/1/right
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIjNDU2IyMj"

### Sample API request to retrieve analisys for {id} 1.

    GET /v1/diff/1
    Host: diff.azurewebsites.net

### Sample response from above analysis.

    {
        "Status": "DifferencesFound",
        "DiffBlocks": [
            {
                "Offset": 2,
                "Lenght": 1
            },
            {
                "Offset": 6,
                "Lenght": 3
            }
        ]
    }
