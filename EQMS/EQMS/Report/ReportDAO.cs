using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using System.Windows;

namespace EQMS.Report
{
    public interface IReportDAO
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        List<(string EquipmentName, int TotalCount)> GetData(string query);
    }
    public class ReportDAO : IReportDAO
    {
        public string _connectionString { get; }

        public ReportDAO(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public List<(string EquipmentName, int TotalCount)> GetData(string query)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    SqlDataReader reader = command.ExecuteReader();
                    var data = new List<(string EquipmentName, int TotalCount)>();
                    while (reader.Read())
                    {

                        string equipmentName = reader["EquipmentName"].ToString();
                        int totalCount = Convert.ToInt32(reader["TotalCount"]);
                        data.Add((equipmentName, totalCount));
                    }

                    return data;
                }
            }
        }


    }
}
