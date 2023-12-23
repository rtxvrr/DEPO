using DEPO.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DEPO.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private DatabaseManager databaseManager;
        public AddEmployeeWindow()
        {
            InitializeComponent();
            databaseManager = new DatabaseManager("Data Source=REVISION-PC\\SQLEXPRESS;Initial Catalog=DEPO;Integrated Security=True");
        }
        private bool IsAlpha(string value)
        {
            return value.All(char.IsLetter);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lastName = LastNameTextBox.Text;
                string firstName = FirstNameTextBox.Text;
                string middleName = MiddleNameTextBox.Text;
                DateTime birthDate = BirthDatePicker.SelectedDate ?? DateTime.Now; // Используем текущую дату, если не выбрана
                string passportSerial = PassportSerialTextBox.Text;
                string passportNumber = PassportNumberTextBox.Text;

                // Проверки на допустимость данных
                if (firstName.Length > 50 || !IsAlpha(firstName))
                {
                    MessageBox.Show("Имя должно быть не более 50 символов и содержать только буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (lastName.Length > 60 || !IsAlpha(lastName))
                {
                    MessageBox.Show("Фамилия должна быть не более 60 символов и содержать только буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (middleName.Length > 60 || !IsAlpha(middleName))
                {
                    MessageBox.Show("Отчество должно быть не более 60 символов и содержать только буквы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Regex.IsMatch(passportSerial, @"^\d{4}$"))
                {
                    MessageBox.Show("Серия паспорта должна состоять из 4 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Regex.IsMatch(passportNumber, @"^\d{6}$"))
                {
                    MessageBox.Show("Номер паспорта должен состоять из 6 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                databaseManager.InsertEmployee(lastName, firstName, middleName, birthDate, passportSerial, passportNumber);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
