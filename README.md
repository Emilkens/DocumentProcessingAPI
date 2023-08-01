# DocumentProcessingAPI 📁

This project is a part of my C#, ASP.NET portfolio. It is based on a recruitment task that was once assigned to [me](https://github.com/Emilkens).

## Overview of the API

This is ASP.NET Core 6 Minimal API project responsible for processing [csv files](https://en.wikipedia.org/wiki/Comma-separated_values) and returning them as [JSON](https://en.wikipedia.org/wiki/JSON) along with file metadata.

## Authentication

API user must authenticate using Basic Auth. Authentication is handled by aprropriate middleware. This was one of the task's requirements.  The expected credentials can be configured by setting the following environment variables:
- ```DOCUMENT_PROCESSING_API_PASSWORD```
- ```DOCUMENT_PROCESSING_API_LOGIN```

## How to test it

By default API will expose its actions on ```https://localhost:7200```.
In order to authenticate, an HTTP request must contain an appropriate authentication header (refer to [HTTP Authentication](https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication)).

 API expects request's body to contain specific ```csv``` text. Example files are located in ```/Postman/TestFiles/``` directory.

### Recommended approach - Postman
It is recommended to use [Postman API Platform](https://www.postman.com/) to test the app. Repository contains ready-to-use HTTP requests covering different scenarios. These are exported to postman_collection.json. In order to use them, ```/Postman/TestFiles/``` folder has to be moved to [Postman's working directory](https://learning.postman.com/docs/getting-started/settings/#working-directory).
