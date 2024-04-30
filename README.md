
## Topup Service 
This repo contains Topup Service with .NET8 Web API and responsible from user beneficiary CRUD and topup operations

**Prerequisities**

.NET8 SDK must be installed

MSSQL Express must be installed and connection string must be modified for local setup for TransactionService project 
and after that in TransactionService.DBMigration project, same connection string must be updated and project must be run locally to create database and seed data.

## Build, Test, Run

Run the following commands from the folder containing the .sln file in order to build and test.

`dotnet build`

`dotnet test`



## Local debugging and testing

**Swagger URL** 
`http://localhost:88/swagger/index.html`
