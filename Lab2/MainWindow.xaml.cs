using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Lab2
{
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Створюємо CommandBinding для кожної з вбудованих команд
            var openBinding = new CommandBinding(ApplicationCommands.Open,
                                                 Execute_Open,
                                                 CanExecute_Open);

            var closeBinding = new CommandBinding(ApplicationCommands.Close,
                                                  Execute_Close,
                                                  CanExecute_Close);

            var saveBinding = new CommandBinding(ApplicationCommands.Save,
                                                 Execute_Save,
                                                 CanExecute_Save);

            // Додаємо їх до колекції CommandBindings вікна
            CommandBindings.Add(openBinding);
            CommandBindings.Add(closeBinding);
            CommandBindings.Add(saveBinding);
        }

        // ======================================================
        //             SAVE
        // ======================================================
        private void CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            // Кнопка Save буде доступна лише тоді, коли в TextBox є текст
            e.CanExecute = MainTextBox.Text.Trim().Length > 0;
        }

        private void Execute_Save(object sender, ExecutedRoutedEventArgs e)
        {
            // Збереження тексту у файл
            File.WriteAllText("Lab2File.txt", MainTextBox.Text);
            MessageBox.Show("The file was saved!");
        }

        // ======================================================
        //             OPEN
        // ======================================================
        private void CanExecute_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            // Наприклад, дозвольмо відкривати файл тільки коли TextBox порожній
            // (щоб не перезаписувати існуючий текст).
            // Якщо вам не потрібно таке обмеження – встановіть true.
            e.CanExecute = MainTextBox.Text.Trim().Length == 0;
        }

        private void Execute_Open(object sender, ExecutedRoutedEventArgs e)
        {
            const string filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            var openDialog = new OpenFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = filter
            };

            if (openDialog.ShowDialog() == true)
            {
                MainTextBox.Text = File.ReadAllText(openDialog.FileName);
            }
        }

        // ======================================================
        //             CLOSE (Clear)
        // ======================================================
        private void CanExecute_Close(object sender, CanExecuteRoutedEventArgs e)
        {
            // Кнопка Close (очистити) доступна, якщо в текстбоксі щось є
            e.CanExecute = MainTextBox.Text.Length > 0;
        }

        private void Execute_Close(object sender, ExecutedRoutedEventArgs e)
        {
            // Очищаємо текст. Якщо хочете закривати вікно,
            // можна замінити на this.Close().
            MainTextBox.Text = "";
        }
    }
}
