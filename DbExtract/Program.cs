using Microsoft.Data.SqlClient;
using System.Text.Json;

var db = new List<DbTable>();

using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=AdventureWorksLT2022;Integrated Security=True;TrustServerCertificate=True"))
{
    // Query database for tables and columns info
    connection.Open();
    string sql = @"SELECT t.Name AS 'TableName', c.Name as 'ColumName', ty.name as 'ColumnType'
                FROM     sys.columns c
                          JOIN sys.tables t ON t.object_id = c.object_id
                          JOIN sys.types ty on ty.user_type_id = c.user_type_id
                WHERE    t.type = 'U'
                ORDER BY t.Name";

    using (SqlCommand command = new SqlCommand(sql, connection))
    {
        using (SqlDataReader reader = command.ExecuteReader())
        {
            // Build schema structure as we read through the rows
            while (reader.Read())
            {
                // Check if we already created a table object for a given table name
                var existingTable = db.FirstOrDefault(x => x.TableName == reader.GetValue(0).ToString());

                // If the table object already exists in the list, just add the next column to it
                if (existingTable != null)
                {
                    existingTable.Columns.Add(
                        new DbColumn() 
                        { 
                            ColumnName = reader.GetValue(1).ToString(),
                            ColumnType = reader.GetValue(2).ToString() 
                        });
                }
                // If a table object with that name doesn't exist in the list, create it
                else
                {
                    db.Add(new DbTable()
                    {
                        TableName = reader.GetValue(0).ToString(),
                        Columns = new List<DbColumn>()
                        {
                            new DbColumn()
                            {
                                ColumnName = reader.GetValue(1).ToString(),
                                ColumnType = reader.GetValue(2).ToString()
                            }
                        }
                    });
                }
            }
        }
    }
}

Console.WriteLine(JsonSerializer.Serialize(db));

public class DbTable()
{
    public string TableName { get; set; }
    public List<DbColumn> Columns { get; set; } = new List<DbColumn>();
}

public class DbColumn()
{
    public string ColumnName { get; set; }
    public string ColumnType { get; set; }
}