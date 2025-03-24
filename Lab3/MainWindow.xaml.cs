using System;
using System.Windows;

namespace FortuneTeller
{
    public partial class MainWindow : Window
    {
        private readonly string[] answers = new string[]
        {
            "Так", "Ні", "Скоріше так", "Скоріше ні", "Можливо", "Запитай пізніше","Це залежить лише від тебе"
        };

        private readonly Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AskFortune_Click(object sender, RoutedEventArgs e)
        {
            string question = QuestionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(question))
            {
                AnswerText.Text = "Будь ласка, введи запитання!";
                return;
            }

            int index = random.Next(answers.Length);
            AnswerText.Text = $"На твоє питання: \"{question}\"\nВідповідь: {answers[index]}";
        }
    }
}
