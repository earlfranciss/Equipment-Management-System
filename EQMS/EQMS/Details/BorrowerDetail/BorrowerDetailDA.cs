using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using EQMS.Borrower;

namespace EQMS.Details.BorrowerDetail
{
    public interface IBorrowerDetailDA
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        bool SaveEdit(string id, string fn, string ln, string email, string phone, string position);
        bool AddBorrower( string fn, string ln, string email, string phone, string role);
        bool DeleteBorrower(string id);
        bool EmailExist(string id, string email);
        bool PhoneExist(string id, string phone);
        void SaveImageToDatabase(int id, byte[] imageData);
        BitmapImage GetImage(int borrowerId);
        EQMS.Borrower.Borrower GetID(int user);
    }
    internal class BorrowerDetailDA : IBorrowerDetailDA
    {
        public string _connectionString { get; }

        public BorrowerDetailDA(string _connectionString)
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

        // Save Edits to Borrower
        public bool SaveEdit(string id, string fn, string ln, string email, string phone, string role)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Borrower SET FirstName = @FirstName, LastName = @LastName, PhoneNo = @Phone, Email = @Email, Type = @Role WHERE BorrowerID = @BorrowerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", fn);
                command.Parameters.AddWithValue("@LastName", ln);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Role", role);
                command.Parameters.AddWithValue("@BorrowerID", Convert.ToInt32(id));

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

        public bool AddBorrower(string fn, string ln, string email, string phone, string role)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "INSERT INTO Borrower ( FirstName, LastName, PhoneNo, Email, Type) VALUES ( @FirstName, @LastName, @Phone, @Email, @Role);";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", fn);
                command.Parameters.AddWithValue("@LastName", ln);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Role", role);

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

        // Deletes an Borrower using BorrowerID
        public bool DeleteBorrower(string id)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "DELETE FROM Borrower WHERE BorrowerID = @BorrowerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BorrowerID", id);

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
                string query = "SELECT * FROM Borrower WHERE Email = @Email AND BorrowerID <> @BorrowerID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@BorrowerID", id);

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
                string query = "SELECT * FROM Borrower WHERE PhoneNo = @Phone AND BorrowerID <> @BorrowerID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@BorrowerID", id);

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

        // Get Borrower Info using ID
        public EQMS.Borrower.Borrower GetID(int user)
        {
            using (SqlConnection connection = GetConnection())
            {
                EQMS.Borrower.Borrower borrower = null;

                string query = "SELECT FirstName, LastName, Email, PhoneNo, Type FROM Borrower WHERE BorrowerID = @BorrowerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BorrowerID", user);


                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        borrower = new EQMS.Borrower.Borrower
                        {
                            ID = user,  
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["PhoneNo"].ToString(),
                            Role = reader["Type"].ToString()
                        };
                        return borrower;
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
                string query = "UPDATE Borrower SET Img = @Photo WHERE BorrowerID = @BorrowerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Photo", imageData);
                command.Parameters.AddWithValue("@BorrowerID", id);

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

        // Load Borrower Image 
        public BitmapImage GetImage(int borrowerId)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT Img FROM Borrower WHERE BorrowerID = @BorrowerID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BorrowerID", borrowerId);

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
