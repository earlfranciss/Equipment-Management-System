using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EQMS.Reservation
{
    public interface IReservationDAO
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        public DataTable GetData(string query);
    }
    internal class ReservationDAO : IReservationDAO
    {
        public string _connectionString { get; }

        public ReservationDAO(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public DataTable GetData(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                try
                {
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return dataTable;
        }


        public DataTable Search(string search, string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SearchText", search + "%");

                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        dataTable.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return dataTable;
            }
        }

        public DataTable MergedData(DataTable table1, DataTable table2)
        {
            DataTable mergedTable = new DataTable();
            mergedTable.Merge(table1);
            mergedTable.Merge(table2);

            return mergedTable;
        }

    }
}
