using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEPO.Classes
{
    public class DatabaseManager
    {
        private readonly string connectionString;
        public event EventHandler DataUpdated;

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }
        protected virtual void OnDataUpdated()
        {
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void InsertOrganization(string name, string inn, string legalAddress, string actualAddress)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Organization (Title, INN, LegalAddress, ActualAddress) " +
                               "VALUES (@Title, @INN, @LegalAddress, @ActualAddress)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", name);
                    cmd.Parameters.AddWithValue("@INN", inn);
                    cmd.Parameters.AddWithValue("@LegalAddress", legalAddress);
                    cmd.Parameters.AddWithValue("@ActualAddress", actualAddress);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertEmployee(string lastName, string firstName, string middleName, DateTime birthDate, string passportSerial, string passportNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Employee (FirstName, LastName, MiddleName, Birthdate, PassportSerial, PassportNumber) " +
                               "VALUES (@FirstName, @LastName, @MiddleName, @Birthdate, @PassportSerial, @PassportNumber)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@MiddleName", middleName);
                    cmd.Parameters.AddWithValue("@Birthdate", birthDate);
                    cmd.Parameters.AddWithValue("@PassportSerial", passportSerial);
                    cmd.Parameters.AddWithValue("@PassportNumber", passportNumber);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ImportEmployeesFromCsv(string filePath)
        {
            try
            {
                List<Employee> employeesData = new List<Employee>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        Employee newEmployee = new Employee
                        {
                            LastName = values[0],
                            FirstName = values[1],
                            MiddleName = values[2],
                            Birthdate = DateTime.Parse(values[3]),
                            PassportSerial = values[4],
                            PassportNumber = values[5]
                        };
                        employeesData.Add(newEmployee);
                        InsertEmployee(newEmployee.LastName, newEmployee.FirstName, newEmployee.MiddleName,
                                        newEmployee.Birthdate.Date, newEmployee.PassportSerial, newEmployee.PassportNumber);
                    }
                }

                // Вызываем событие для уведомления об изменении данных
                OnDataUpdated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка импорта сотрудников: {ex.Message}");
            }
        }

        public void ExportEmployeesToCsv(string filePath)
        {
            try
            {
                List<Employee> employeesData = GetEmployees();

                List<string> lines = new List<string>();

                foreach (var employeeData in employeesData)
                {
                    string line = $"{employeeData.LastName},{employeeData.FirstName},{employeeData.MiddleName}," +
                                  $"{employeeData.Birthdate.Date},{employeeData.PassportSerial},{employeeData.PassportNumber}";
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка экспорта сотрудников: {ex.Message}");
            }
        }
        public void ImportOrganizationsFromCsv(string filePath)
        {
            try
            {
                List<Organization> organizationsData = new List<Organization>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        Organization newOrganization = new Organization
                        {
                            Title = values[0],
                            INN = values[1],
                            LegalAddress = values[2],
                            ActualAddress = values[3]
                        };

                        organizationsData.Add(newOrganization);
                        InsertOrganization(newOrganization.Title, newOrganization.INN, newOrganization.LegalAddress, newOrganization.ActualAddress);
                    }
                }

                // Вызываем событие для уведомления об изменении данных
                OnDataUpdated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка импорта организаций: {ex.Message}");
            }
        }

        public void ExportOrganizationsToCsv(string filePath)
        {
            try
            {
                List<Organization> organizationsData = GetOrganizations();

                List<string> lines = new List<string>();

                foreach (var organizationData in organizationsData)
                {
                    string line = $"{organizationData.Title},{organizationData.INN},{organizationData.LegalAddress},{organizationData.ActualAddress}";
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка экспорта организаций: {ex.Message}");
            }
        }
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Employee";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                ID = (int)reader["ID"],
                                LastName = reader["LastName"].ToString(),
                                FirstName = reader["FirstName"].ToString(),
                                MiddleName = reader["MiddleName"].ToString(),
                                Birthdate = (DateTime)reader["Birthdate"],
                                PassportSerial = reader["PassportSerial"].ToString(),
                                PassportNumber = reader["PassportNumber"].ToString()
                            });
                        }
                    }
                }
            }

            return employees;
        }

        public List<Organization> GetOrganizations()
        {
            List<Organization> organizations = new List<Organization>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Organization";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            organizations.Add(new Organization
                            {
                                ID = (int)reader["ID"],
                                Title = reader["Title"].ToString(),
                                INN = reader["INN"].ToString(),
                                LegalAddress = reader["LegalAddress"].ToString(),
                                ActualAddress = reader["ActualAddress"].ToString()
                            });
                        }
                    }
                }
            }

            return organizations;
        }
    }
    public class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthdate { get; set; }
        public string PassportSerial { get; set; }
        public string PassportNumber { get; set; }
    }

    public class Organization
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string INN { get; set; }
        public string LegalAddress { get; set; }
        public string ActualAddress { get; set; }
    }
}
