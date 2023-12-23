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
using DEPO.Pages;

namespace DEPO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrganizationBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new OrganizationPage());
        }

        private void EmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new EmployeePage());
        }
    }
}
