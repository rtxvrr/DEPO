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
    /// Логика взаимодействия для AddOrganizationWindow.xaml
    /// </summary>
    public partial class AddOrganizationWindow : Window
    {
        private DatabaseManager databaseManager;
        public AddOrganizationWindow()
        {
            InitializeComponent();
            databaseManager = new DatabaseManager("Data Source=REVISION-PC\\SQLEXPRESS;Initial Catalog=DEPO;Integrated Security=True");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameTextBox.Text;
                string inn = INNTextBox.Text;
                string legalAddress = LegalAddressTextBox.Text;
                string actualAddress = ActualAddressTextBox.Text;

                // Проверки на допустимость данных
                if (name.Length > 100)
                {
                    MessageBox.Show("Название организации должно быть не более 100 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!Regex.IsMatch(inn, @"^\d{12}$"))
                {
                    MessageBox.Show("ИНН должен состоять из 12 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (legalAddress.Length > 150)
                {
                    MessageBox.Show("Юридический адрес должен быть не более 150 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (actualAddress.Length > 150)
                {
                    MessageBox.Show("Фактический адрес должен быть не более 150 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                databaseManager.InsertOrganization(name, inn, legalAddress, actualAddress);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении организации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
