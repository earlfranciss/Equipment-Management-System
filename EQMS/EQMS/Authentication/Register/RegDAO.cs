using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EQMS.Authentication.Register
{
    public interface IRegDAO
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        void RegisterDB(string fn, string ln, string email, string phone, string pass);
        bool EmailExist(string email);
        bool PhoneExist(string phone);
        string GetUserID(string fn, string ln);
    }

    internal class RegDAO : IRegDAO
    {
        public string _connectionString { get; }

        public RegDAO(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public void RegisterDB(string fn, string ln, string email, string phone, string pass)
        {
            pass = EncryptPassword(pass);
            if (!EmailExist(email) && !PhoneExist(phone))
            {
                using(SqlConnection connection = GetConnection())
                {
                    string query = "INSERT INTO Employee (FirstName, LastName, Email,  Password, PhoneNo, Position) VALUES (@FirstName, @LastName, @Email,  @Password, @PhoneNumber, @Position)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", fn);
                    command.Parameters.AddWithValue("@LastName", ln);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", phone);
                    command.Parameters.AddWithValue("@Password", pass);
                    command.Parameters.AddWithValue("@Position", "staff");
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
        }

        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool EmailExist(string email)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE Email = @Email;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

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
        public bool PhoneExist(string phone)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE PhoneNo = @Phone;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Phone", phone);

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

        public string GetUserID(string fn, string ln)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE FirstName = @FirstName AND LastName = @LastName;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", fn);
                command.Parameters.AddWithValue("@LastName", ln);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader.GetInt32(reader.GetOrdinal("EmployeeID")).ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return "";
                }
            }
        }
    }
}
