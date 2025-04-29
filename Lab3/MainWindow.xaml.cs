using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FortuneTeller
{
    public partial class MainWindow : Window
    {
        private readonly string[] answers = new[]
        {
            "Так", "Скоро дізнаєшся відповідь.", "Зараз не час для цього.",
            "Ні", "Удача на твоєму боці!", "Скоріше так", "Скоріше ні"
        };

        private readonly Random random = new Random();
        private readonly string dbPath = "fortunes.db";

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadHistory();
        }

        private void AskFortune_Click(object sender, RoutedEventArgs e)
        {
            string question = QuestionTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(question))
            {
                string randomAnswer = answers[random.Next(answers.Length)];
                AnswerText.Text = randomAnswer;
                AskButton.IsEnabled = false;

                SaveFortune(question, randomAnswer);
                LoadHistory();
            }
        }

        private void QuestionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AskButton.IsEnabled = !string.IsNullOrWhiteSpace(QuestionTextBox.Text);
            AnswerText.Text = "Тут буде відповідь...";
        }

        private void InitializeDatabase()
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

        private void SaveFortune(string question, string answer)
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

        private void LoadHistory()
        {
            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();

                using (var adapter = new SQLiteDataAdapter("SELECT * FROM FortuneHistory ORDER BY Id DESC", conn))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    HistoryGrid.ItemsSource = DatabaseHelper.GetFortunes().DefaultView;
                }
            }
        }
        private void HistoryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HistoryGrid.SelectedItem is DataRowView row)
            {
                QuestionTextBox.Text = row["Question"].ToString();
                AnswerText.Text = row["Answer"].ToString();
            }
        }


    }

}
