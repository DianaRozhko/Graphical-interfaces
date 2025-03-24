using System;
using System.Windows;

namespace FortuneTeller
{
    public partial class MainWindow : Window
    {
        // Масив можливих відповідей від ворожки
        private readonly string[] answers = new string[]
        {
            "Так", "Ні", "Скоріше так", "Скоріше ні", "Можливо", "Запитай пізніше", "Це залежить лише від тебе"
        };

        // Генератор випадкових чисел
        private readonly Random random = new Random();

        // Конструктор головного вікна
        public MainWindow()
        {
            InitializeComponent(); // Ініціалізація компонентів інтерфейсу
        }

        // Обробник натискання кнопки
        private void AskFortune_Click(object sender, RoutedEventArgs e)
        {
            // Зчитування запитання користувача
            string question = QuestionTextBox.Text.Trim();

            // Якщо поле пусте — вивести підказку
            if (string.IsNullOrEmpty(question))
            {
                AnswerText.Text = "Будь ласка, введи запитання!";
                return;
            }

            // Випадковий вибір відповіді
            int index = random.Next(answers.Length);
            AnswerText.Text = $"На твоє питання: \"{question}\"\nВідповідь: {answers[index]}";
        }
    }
}
