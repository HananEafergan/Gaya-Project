# Gaya-Project

A calculator whose operators live in SQL Server LocalDB. Add and delete show or hide an existing operator by setting `IsActive`. They do not create a new operation.

## Run

Requires the .NET 10 SDK and SQL Server LocalDB. The connection string in `Gaya-Server/appsettings.json` uses database `GayaDB`.

Apply the migrations in `Gaya-Server/Migrations`, then start the app:

```
dotnet run --project Gaya-Server --launch-profile http
```

Open http://localhost:5199/.

Migrations create empty tables. The page can only show or hide rows that already exist, and calculation only handles these names:

```sql
INSERT INTO Operators (Name, IsActive) VALUES
('Add', 1),
('Subtract', 1),
('Multiply', 1),
('Divide', 1),
('Concat', 1);
```

## Behavior

The operator dropdown lists active names from `GetActiveOperators`. **Edit Operators** lists every row. Activate calls `PUT /api/GayaProject/AddOperator/{id}`. Deactivate calls `DELETE /api/GayaProject/DeleteOperator/{id}`. The dropdown reloads after each change.

**Calculate** stays disabled when either field is empty, no operator is selected, no operators are active, or edit mode is open. A successful call shows the current result, the three calculations saved before it, and how many times that operator was used this month (including the one just saved). API errors, such as invalid numbers or division by zero, appear above the result.

## Layout

`wwwroot` is the page. `Controllers`, `BLL`, and `DAL` are the API. `Filters/RequestResponseLoggingFilter` logs each controller request and response. Serilog writes the logs to the `Logs` directory using daily rolling log files.
