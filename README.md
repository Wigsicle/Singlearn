Pull the project, Install MSSQL, SSMS Get login credentials from SSMS Place the below in your appsettings.development.json { "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } }, "AllowedHosts": "*",

"ConnectionStrings": { "DefaultConnection": "Server=FARM\SQLEXPRESS;Database=SinglearnDB; Trusted_Connection=True; TrustServerCertificate=True" } }

Change FARM\SQLEXPRESS to the server you see in the SSMS login.

In VS -> Tools -> NuGet Package Manager -> Package Manager Console

In Package Manager Console, type Update-Database (Remember the dash)

You don't need to create the database. The migration will do that for you.

This will do the db migration.

You'll need to create a user -> staff -> class -> student (In this order, due to the FK).

You may be confused in the way the queries are written. I would just right click on the table you want to enter data into -> Script Table As -> INSERT To

Here is an example format:

USE [SinglearnDB]
GO

INSERT INTO [dbo].[Staff]
           ([staff_id]
           ,[user_id]
           ,[name]
           ,[contact_no])
     VALUES
           ('1', -- Replace with actual staff_id value (nvarchar(450))
           '628A9687-DA0B-4CE6-9CFD-6357AF255E47', -- Replace with actual user_id value (nvarchar(max))
           'Richard', -- Replace with actual name value (nvarchar(max))
           '985') -- Replace with actual contact_no value (nvarchar(max))
GO
