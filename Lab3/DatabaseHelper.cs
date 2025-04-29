using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace FortuneTeller
{
    public static class DatabaseHelper
    {
        private static readonly string dbPath = "fortunes.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    string sql = @"
                        CREATE TABLE FortuneHistory (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Question TEXT,
                            Answer TEXT,
                            Date TEXT
                        )";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveFortune(string question, string answer)
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                string sql = "INSERT INTO FortuneHistory (Question, Answer, Date) VALUES (@q, @a, @d)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@q", question);
                    cmd.Parameters.AddWithValue("@a", answer);
                    cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetFortunes()
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                using (var adapter = new SQLiteDataAdapter("SELECT * FROM FortuneHistory ORDER BY Id DESC", conn))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
