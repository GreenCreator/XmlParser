using System;
using System.Data.SQLite;
using System.IO;

namespace Xml.io.helper
{
    public class DatabaseHelper
    {
        public DatabaseHelper(string path)
        {
            InitializeDatabase(path);
        }

        private void InitializeDatabase(string databasePath)
        {
            if (!File.Exists(databasePath))
            {
                using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Files (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            FileName TEXT,
                            FileContent BLOB
                        );";
                    using (var command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Database and table created successfully.");
            }
            else
            {
                Console.WriteLine("Database already exists.");
            }
        }
    }
}