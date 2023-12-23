using DEPO.Classes;
using DEPO.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DEPO.Pages
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        private readonly DatabaseManager databaseManager;
        public EmployeePage()
        {
            InitializeComponent();
            
            databaseManager = new DatabaseManager("Data Source=REVISION-PC\\SQLEXPRESS;Initial Catalog=DEPO;Integrated Security=True");
            databaseManager.DataUpdated += DatabaseManager_DataUpdated;
        }
        private void DatabaseManager_DataUpdated(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }
        private void RefreshDataGrid()
        {

            employeeDataGrid.ItemsSource = databaseManager.GetEmployees();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }
        private void ImportEmployees_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                databaseManager.ImportEmployeesFromCsv(filePath);
            }
        }

        private void ExportEmployees_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.FileName = "Export EmployeeData";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                databaseManager.ExportEmployeesToCsv(filePath);
            }
        }

        private void employeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем и отображаем диалоговое окно для добавления нового сотрудника
            AddEmployeeWindow addEmployeeDialog = new AddEmployeeWindow();
            addEmployeeDialog.ShowDialog();
            RefreshDataGrid();
        }
    }
}
