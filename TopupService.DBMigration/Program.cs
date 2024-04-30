// See https://aka.ms/new-console-template for more information
using TopupService.DBMigration;

Console.WriteLine("Inits SQL DB for the service");

// Call DbUp migration
var connectionString = "Server=localhost\\SQLEXPRESS;Database=TopupDb;Trusted_Connection=True;TrustServerCertificate=True";
// todo: migrate to an app config 
MsSqlMigrator.Execute(connectionString);