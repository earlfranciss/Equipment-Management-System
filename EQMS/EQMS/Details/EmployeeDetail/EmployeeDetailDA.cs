using System;
using EQMS.Employee;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;

namespace EQMS.Details.EmployeeDetail
{
    public interface IEmployeeDetailDA
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        bool SaveEdit(string id, string fn, string ln, string email, string phone, string position);
        bool DeleteEmployee(string id);
        bool EmailExist(string id, string email);
        bool PhoneExist(string id, string phone);
        void SaveImageToDatabase(int id, byte[] imageData);
        BitmapImage GetImage(int employeeId);
        EQMS.Employee.Employee GetID(int user);
    }
    internal class EmployeeDetailDA : IEmployeeDetailDA
    {
        public string _connectionString { get; }

        public EmployeeDetailDA(string _connectionString)
        {
            this._connectionString = _connectionString;
        }


        // Get Connection to Database
        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // Save Edits to Employee
        public bool SaveEdit(string id, string fn, string ln, string email, string phone, string position)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, PhoneNo = @Phone, Email = @Email, Position = @Position WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", fn);
                command.Parameters.AddWithValue("@LastName", ln);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Position", position);
                command.Parameters.AddWithValue("@EmployeeID", Convert.ToInt32(id));

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        
        // Deletes an Employee using EmployeeID
        public bool DeleteEmployee(string id)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        // Checks if Email already exists
        public bool EmailExist(string id, string email)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE Email = @Email AND EmployeeID <> @EmployeeID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        // Checks if Phone Number already exists
        public bool PhoneExist(string id, string phone)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE PhoneNo = @Phone AND EmployeeID <> @EmployeeID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        // Get Employee Info using ID
        public EQMS.Employee.Employee GetID(int user)
        {
            using (SqlConnection connection = GetConnection())
            {
                EQMS.Employee.Employee employee = null;

                string query = "SELECT FirstName, LastName, Email, PhoneNo, Position FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", user);


                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        employee = new EQMS.Employee.Employee
                        {
                            ID = user,
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["PhoneNo"].ToString(),
                            Position = reader["Position"].ToString()
                        };
                        return employee;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
        }


        // Save image data to Database
        public void SaveImageToDatabase(int id, byte[] imageData)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Employee SET Img = @Photo WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Photo", imageData);
                command.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Load Employee Image 
        public BitmapImage GetImage(int employeeId)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT Img FROM Employee WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["Img"] != DBNull.Value)
                    {
                        byte[] imageData = (byte[])reader["Img"];
                        BitmapImage bitmap = new BitmapImage();
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = ms;
                            bitmap.EndInit();
                        }
                        return bitmap;
                    }
                }
            }
            return null;
        }
    }
}
