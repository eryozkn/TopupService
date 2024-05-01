
## Topup Service 
This repo contains Topup Service with .NET8 Web API and responsible from user beneficiary CRUD and topup operations

**Prerequisities**

.NET8 SDK must be installed

MSSQL Express must be installed and connection string must be modified for local setup for TransactionService project 
and after that in TransactionService.DBMigration project, same connection string must be updated and project must be run locally to create database and seed data.

Transaction service locally must be up and running from https://localhost:81 

## Build, Test, Run

Run the following commands from the folder containing the .sln file in order to build and test.

`dotnet build`

`dotnet test`



## Local debugging and testing

**Swagger URL** 
`https://localhost:88/swagger/index.html`


## Overall architecture of a Topup application

![image](https://github.com/eryozkn/TopupService/assets/1847583/940acf01-bc41-45f9-a528-ba03c5f33e54)

