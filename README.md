# Diff API

This API  receive two blocks of binary data, enconded in a base 64 string, and compare the differences bewteen them. One block is named as "left" and the other as "right". After the two blocks are known by the API, the analysis of the diferences could be executed.

The results of the analisys happens accordingly to below conditions:
- In case of *"left"* and *"right"* data have the same number of bytes, the analisys will be executed. If some difference is found, the status returned will be *"**DifferencesFound**"*, and a list with the *offset* (where the difference starts) and the *lenght* (how long is the difference in that block) will be returned also;
- If *"left"* and *"right"* data are equals (same size and sequence), the status *"**DifferencesNotFound**"* will be returned;
- In case of *"left"* and *"right"* data don't be of the same size, the analisys will not proceed and the status *"**BlocksAreNotOfSameSize**"* will be returned.

The Diff API is available online at http://diff.azurewebsites.net/ and the the UI to execute some tests is available at http://diff.azurewebsites.net/swagger

## Sample analysis execution for *"DifferencesFound"*

Below, two blocks of data encoded in a base 64 string will be sent to analysis.
The *"left"*  data will be *123456789* and the *"right"* data will be *12#456###*.
Since the blocks are of the same size, the analisys will be executed. We can observe two differences between these blocks, the first at index 2 with only one different character and the second at index 6 with three differentes characters.

In order to execute the analisys we will need three calls to API, as show below.

### First Call - Update *"left"*  data using {id} 1
Data being sent: 123456789

    POST /v1/diff/1/left
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

### Second Call - Update *"right"*  data using {id} 1
Data being sent: 12#456###

    POST /v1/diff/1/right
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIjNDU2IyMj"

### Third Call - Execute the analisys for {id} 1.

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

## Sample analysis execution for *"DifferencesNotFound"*

Below, two blocks of data encoded in a base 64 string will be sent to analysis.
The *"left"*  data will be *123456789* and the *"right"* data will be *123456789*.
Since the blocks are of the same size, the analisys will be executed, but as they have exactly the same sequence of bytes, the retuned status will be *"DifferencesNotFound"*.

In order to execute the analisys we will need three calls to API, as show below.

### First Call - Update *"left"*  data using {id} 1
Data being sent: 123456789

    POST /v1/diff/1/left
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

### Second Call - Update *"right"*  data using {id} 1
Data being sent: 123456789

    POST /v1/diff/1/right
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

### Third Call - Execute the analisys for {id} 1.

    GET /v1/diff/1
    Host: diff.azurewebsites.net

###### Response from above analysis.

    {
        "Status": "DifferencesNotFound",
        "DiffBlocks": []
    }

## Sample analysis execution for *"BlocksAreNotOfSameSize"*

Below, two blocks of data encoded in a base 64 string will be sent to analysis.
The *"left"*  data will be *123456* and the *"right"* data will be *123456789*.
Since the blocks aren't of the same size, the analisys for differents blocks will not be exeuted and the retuned status will be *"BlocksAreNotOfSameSize"*.

In order to execute the analisys we will need three calls to API, as show below.

### First Call - Update *"left"*  data using {id} 1
Data being sent: 123456

    POST /v1/diff/1/left
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2"

### Second Call - Update *"right"*  data using {id} 1
Data being sent: 123456789

    POST /v1/diff/1/right
    Host: diff.azurewebsites.net
    Content-Type: application/json
    "MTIzNDU2Nzg5"

### Third Call - Execute the analisys for {id} 1.

    GET /v1/diff/1
    Host: diff.azurewebsites.net

###### Response from above analysis.

    {
        "Status": "BlocksAreNotOfSameSize",
        "DiffBlocks": []
    }

## Parameters descriptions

List of parameters per endpoint.

`POST /v1/diff/{id}/left `

`POST /v1/diff/{id}/right`

| Parameter  | Sample Value | Description | Parameter Type | Data Type |
| :------------ | :------------ | :------------ | :------------ | :------------ |
| Id | 1 | Identify one block that are going to be diff-ed. | path | integer |
| data | "MTIzNDU2Nzg5" | Base64 string to be diff-ed. | body | string |

`GET /v1/diff/{id}`

| Parameter  | Sample Value | Description | Parameter Type | Data Type |
| :------------ | :------------ | :------------ | :------------ | :------------ |
| Id | 1 | Identify one block that are going to be diff-ed. | path | integer |

## Possible exceptions

List of possible exceptions per endpoint.

`POST /v1/diff/{id}/left `

`POST /v1/diff/{id}/right`

| HTTP Status Code  |  Reason |
| :------------ | :------------ |
|400| The parameter {data} cannot be null. |
|400| The parameter {data} is not a valid base 64 string. |
|500| Internal Server Error   |

`GET /v1/diff/{id}`

| HTTP Status Code  |  Reason |
| :------------ | :------------ |
|404| No data was found for provided {id}   |
|500| Internal Server Error   |
