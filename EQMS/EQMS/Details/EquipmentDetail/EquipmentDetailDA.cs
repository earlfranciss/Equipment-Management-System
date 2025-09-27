using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace EQMS.Details.EquipmentDetail
{
    public interface IEquipmentDetailDA
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        bool SaveEdit(string id, string name, string type, string brand, string serial);
        bool DeleteEquipment(string id);
        bool SerialExist(string id, string serial);
        void SaveImageToDatabase(int id, byte[] imageData);
        BitmapImage GetImage(int equipmentId);
        EQMS.Equipment.Equipment GetID(int user);
    }
    internal class EquipmentDetailDA : IEquipmentDetailDA
    {
        public string _connectionString { get; }

        public EquipmentDetailDA(string _connectionString)
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

        // Save Edits to Equipment
        public bool SaveEdit(string id, string name, string type, string brand, string serial)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Equipment SET Name = @Name, Type = @Type, Brand = @Brand, SerialNo = @Serial WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@Brand", brand);
                command.Parameters.AddWithValue("@Serial", serial);
                command.Parameters.AddWithValue("@EquipmentID", Convert.ToInt32(id));

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

        // Deletes an Equipment using EquipmentID
        public bool DeleteEquipment(string id)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "DELETE FROM Equipment WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", id);

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

        // Checks if Serial number already exists
        public bool SerialExist(string id, string serial)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM Equipment WHERE SerialNo = @Serial AND EquipmentID <> @EquipmentID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Serial", serial);
                command.Parameters.AddWithValue("@EquipmentID", id);

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

        // Get Equipment Info using ID
        public EQMS.Equipment.Equipment GetID(int user)
        {
            using (SqlConnection connection = GetConnection())
            {
                EQMS.Equipment.Equipment equipment = null;

                string query = "SELECT Name, Type, Brand, SerialNo FROM Equipment WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", user);


                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        equipment = new EQMS.Equipment.Equipment
                        {
                            ID = user,
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            Brand = reader["Brand"].ToString(),
                            Serial = reader["SerialNo"].ToString(),
                        };
                        return equipment;
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
                string query = "UPDATE Equipment SET Img = @Photo WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Photo", imageData);
                command.Parameters.AddWithValue("@EquipmentID", id);

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

        // Load Equipment Image 
        public BitmapImage GetImage(int equipmentId)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT Img FROM Equipment WHERE EquipmentID = @EquipmentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentID", equipmentId);

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
