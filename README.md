# Address Book REST API
 A REST API built to practice .NET Core fundamentals with Authentication and Authorization, Swagger Doc integration and Unit tests.

[![forthebadge](https://forthebadge.com/images/badges/made-with-c-sharp.svg)](https://forthebadge.com)

Features
- A RESTful Web api with .NET CORE 6 using postgresql Database
- Documented with Swagger with Open API Specification V3 using Swashbuckle
- Authentication and Authorization enabled using JWT Tokens for Access and refresh tokens
- Unit tests using XUnit.

![](header.png)

## Image screenshots

![APIs](https://user-images.githubusercontent.com/45427686/204125818-250711a6-f561-43be-81e4-73aad4f3e36c.png)

```
## Development setup

Install .NET Core 6 and Navigate to the project directory and use the following command to install dependant packages

```sh
dotnet restore
```

Launch the application from Visual Studio or run from the command line using the following command.

```sh
dotnet run
```
To run unit tests, use the following command

```sh
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

## Author

Vidyalakshimi Palanivel

