
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EQMS.Authentication.Login
{
    public interface ILoginDAO
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        bool Authenticate(string user, string pass);
        bool IDNumExist(string user);
    }
    public class LoginDAO : ILoginDAO
    {
        public string _connectionString { get; }


        public LoginDAO(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);
            builder.ConnectRetryCount = 3; 
            builder.ConnectRetryInterval = 10;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            return connection;
        }

        public bool Authenticate(string user, string pass)
        {
            pass = EncryptPassword(pass);
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE EmployeeID = @Username AND Password = @Password;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user);
                command.Parameters.AddWithValue("@Password", pass);

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

        public bool IDNumExist(string user)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Employee WHERE EmployeeID = @Username;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user);

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
    }
}
