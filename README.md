# Database Schema to JSON

A simple tool to extract a database schema as json. The JSON resembles the following structure:

```
[{
  "TableName":"Address",
  "Columns": [
    {
      "ColumnName":"AddressID",
      "ColumnType":"int"
    },
    {
      "ColumnName":"AddressLine1"
      ,"ColumnType":"nvarchar"
    }
  ]
}]
```

The table name is included along with a list of the column names and their data types.

## How to use

Simply replace the connection string in the code with your own database connection string and run the app.
