# Diff API
This API  receive two blocks of binary data, enconded in a base 64 string, and compare the differences bewteen them. One block is named as "left" and the other as "right". After the two blocks are known by the API, the analysis of the diferences could be executed.


The results of the analisys happens accordingly to below conditions:
- If *"left"* and *"right"* data are equals (same size and sequence), the status *"DifferencesNotFound"* will be returned;
- In case of *"left"* and *"right"* data don't be of same size, the analisys will not proceed and the status *"BlocksAreNotOfSameSize"* will be returned;
- In case of *"left"* and *"right"* data have the same number of bytes, the analisys will be executed. The status reruned will be *"DifferencesFound"*, and a list with the *offset* (where the difference starts) and the *lenght* (how long is the difference in that block) will be returned also.

The Diff API is available online at endpoint http://diff.azurewebsites.net/

The UI to execute some tests are available at http://diff.azurewebsites.net/swagger

## Sample analysis execution

Below, two blocks of data encoded in a base 64 string will be sent to analysis.
The "left" data will be *123456789* and the right data will be *12#456###*.
Since the blocks are of the same size, the analisys will be exeuted. We can observe two differences between these blocks, the first at index 2 and with only one character different and the second one at index 6 with three characters differentes.

In order to execute the analisys we will need three call to API, as show below.

#### First Call - Update *"left"* data using {id} 1
Data being sent: 123456789

    POST /v1/diff/1/left
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

#### Second Call - Update *"right"* data using {id} 1
Data being sent: 12#456###

    POST /v1/diff/1/right
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIjNDU2IyMj"

#### Third Call - Execute the analisys for {id} 1.

    GET /v1/diff/1
    Host: diff.azurewebsites.net

###### Response from above analysis.

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
