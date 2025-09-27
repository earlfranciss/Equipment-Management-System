using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EQMS.Authentication.ChangePassword
{
    public interface ICPDAO
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        void ChangePassDB(string user, string pass);
        bool IDNumExist(string user);
    }

    public class CPDAO : ICPDAO
    {
        public string _connectionString { get; }

        public CPDAO(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        public void ChangePassDB(string user, string pass)
        {
            pass = EncryptPassword(pass);
            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Employee SET Password = @NewPassword WHERE EmployeeID = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user);
                command.Parameters.AddWithValue("@NewPassword", pass);
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
