using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Xml.io.helper;

namespace Xml.io.controller;

public class DataAccess
{
    private readonly string _databasePath;

    public DataAccess(string databasePath)
    {
        new DatabaseHelper(databasePath);
        _databasePath = databasePath;
    }

    public void SaveFileToDatabase(string fileName, byte[] fileContent)
    {
        using (var connection = new SQLiteConnection($"Data Source={_databasePath}"))
        {
            connection.Open();
            var command =
                new SQLiteCommand("INSERT INTO Files (FileName, FileContent) VALUES (@FileName, @FileContent)",
                    connection);
            command.Parameters.AddWithValue("@FileName", fileName);
            command.Parameters.AddWithValue("@FileContent", fileContent);
            command.ExecuteNonQuery();
        }
    }

    public IEnumerable<string> GetAllFileContents()
    {
        var contents = new List<string>();
        using (var connection = new SQLiteConnection($"Data Source={_databasePath}"))
        {
            connection.Open();
            var command = new SQLiteCommand("SELECT FileContent FROM Files", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    byte[] fileContent = reader["FileContent"] as byte[];
                    if (fileContent != null)
                    {
                        contents.Add(Encoding.Unicode.GetString(fileContent).TrimStart('\uFEFF'));
                    }
                }
            }
        }

        return contents;
    }
}