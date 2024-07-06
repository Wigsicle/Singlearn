Pull the project, Install MSSQL, SSMS Get login credentials from SSMS Place the below in your appsettings.development.json { "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "AllowedHosts": "*",

"ConnectionStrings": { "DefaultConnection": "Server=FARM\SQLEXPRESS;Database=SinglearnDB; Trusted_Connection=True; TrustServerCertificate=True" } }

Change FARM\SQLEXPRESS to the server you see in the SSMS login.

In VS -> Tools -> NuGet Package Manager -> Package Manager Console

In Package Manager Console, type Update-Database (Remember the dash)

You don't need to create the database. The migration will do that for you.

This will do the db migration.

You'll need to create a user -> staff -> class -> student (In this order, due to the FK).
