using System;
using System.ComponentModel;
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
using LiveCharts;
using System.Collections.ObjectModel;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using EQMS.DatabaseManager;

namespace EQMS.Report
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Page
    {
        public readonly ReportViewModel repVM = new ReportViewModel();
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly dbm db = new dbm();

        public SeriesCollection Values { get; set; }
        public List<string> Labels { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Label { get; set; }
        public Report()
        {
            InitializeComponent();
            LoadDataFromDatabase();
            LoadDataLineGraph();
            SetListNumberUse();
            RecentBorrowed();
            DataContext = this;
        }

        //Bar Graph
        private void LoadDataFromDatabase()
        {

            string query = @"
        WITH CheckoutCounts AS (
            SELECT e.Name AS EquipmentName,
                   COUNT(c.ReservationID) AS CheckoutCount
            FROM Checkout c
            LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID
            LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID
            GROUP BY e.Name),
        ReservationCounts AS (
            SELECT e.Name AS EquipmentName,
                   COUNT(DISTINCT r.ReservationID) AS ReservationCount
            FROM Reservation r
            LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID
            LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID
            WHERE c.ReservationID IS NULL
            GROUP BY e.Name)
        SELECT e.Name AS EquipmentName,
               COALESCE(cc.CheckoutCount, 0) + COALESCE(rc.ReservationCount, 0) AS TotalCount
        FROM Equipment e
        LEFT JOIN ReservationCounts rc ON e.Name = rc.EquipmentName
        LEFT JOIN CheckoutCounts cc ON e.Name = cc.EquipmentName;";

            var equipmentNames = new List<string>();
            var totalCounts = new List<int>();

            using (SqlConnection connection = new SqlConnection(db.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Ensure reader has data
                        if (reader.HasRows)
                        {
                            // Safely read values and check for null
                            int totalCount = reader.IsDBNull(reader.GetOrdinal("TotalCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalCount"));
                            if (totalCount > 0)
                            {
                                equipmentNames.Add(reader["EquipmentName"].ToString());
                                totalCounts.Add(totalCount);
                            }
                        }
                    }
                }
            }

            Labels = equipmentNames;
            Values = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Total Count",
                    Values = new ChartValues<int>(totalCounts),
                    Fill = new SolidColorBrush(Color.FromArgb(180, 36, 51, 106)),
                }
            };
        }


        //Line Graph
        private void LoadDataLineGraph()
        {

            string query = @"
            SELECT 
                DATENAME(MONTH, ReservationDate) AS Month,
                COUNT(*) AS TotalReservations
            FROM 
                Reservation
            GROUP BY 
                DATENAME(MONTH, ReservationDate)
            ORDER BY 
                MIN(ReservationDate);";

            var months = new List<string>();
            var totalReservations = new List<int>();

            using (SqlConnection connection = new SqlConnection(db.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        months.Add(reader["Month"].ToString());
                        totalReservations.Add(Convert.ToInt32(reader["TotalReservations"]));
                    }
                }
            }

            Label = months.ToArray();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Total Reservations",
                    Values = new ChartValues<int>(totalReservations)
                }
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void SetListNumberUse()
        {
            if (Labels != null && Values != null)
            {
                // Clear previous text if any
                txtEquipmentName.Text = "";
                txtTotalCount.Text = "";

                // Iterate through each equipment name and total count
                for (int i = 0; i < Labels.Count; i++)
                {
                    if (i == Labels.Count - 1)
                    {
                        // Append equipment name to the txtEquipmentName TextBlock
                        txtEquipmentName.Text += Labels[i];

                        // Append total count to the txtTotalCount TextBlock
                        txtTotalCount.Text += Values[0].Values[i]; // Assuming only one series
                    }
                    else
                    {
                        // Append equipment name to the txtEquipmentName TextBlock
                        txtEquipmentName.Text += Labels[i] + "\n";

                        // Append total count to the txtTotalCount TextBlock
                        txtTotalCount.Text += Values[0].Values[i] + "\n"; // Assuming only one series
                    }
                }
            }
        }

        public void RecentBorrowed()
        {
            string query = "WITH LatestReservations AS (\r\n    SELECT \r\n        EquipmentID,\r\n        CONVERT(DATE, ReservationDate) AS ReservationDate,\r\n        ROW_NUMBER() OVER (PARTITION BY CONVERT(DATE, ReservationDate) ORDER BY ReservationDate DESC) AS RowNum\r\n    FROM \r\n        Reservation\r\n)\r\nSELECT \r\n    e.Name AS EquipmentName,\r\n    lr.ReservationDate AS ReservationDate\r\nFROM \r\n    LatestReservations lr\r\nJOIN \r\n    Equipment e ON lr.EquipmentID = e.EquipmentID\r\nWHERE \r\n    lr.RowNum = 1\r\nORDER BY \r\n    lr.ReservationDate DESC; -- Order by ReservationDate in descending order\r\n";


            using (SqlConnection connection = new SqlConnection(db.connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Initialize variables to store equipment names and reservation dates
                    List<string> equipmentNames = new List<string>();
                    List<string> reservationDates = new List<string>();

                    // Read up to 3 rows
                    int count = 0;
                    while (reader.Read() && count < 3)
                    {
                        // Safely read values and check for null
                        string equipmentName = reader.IsDBNull(reader.GetOrdinal("EquipmentName")) ? "" : reader.GetString(reader.GetOrdinal("EquipmentName"));
                        string reservationDate = reader.IsDBNull(reader.GetOrdinal("ReservationDate")) ? "" : reader.GetDateTime(reader.GetOrdinal("ReservationDate")).ToString("yyyy-MM-dd");

                        // Add equipment name and reservation date to the lists
                        equipmentNames.Add(equipmentName);
                        reservationDates.Add(reservationDate);

                        // Increment count
                        count++;
                    }

                    // Assign equipment names to the text blocks
                    txtEquipmentName1.Text = equipmentNames.Count > 0 ? equipmentNames[0] : "";
                    txtEquipmentName2.Text = equipmentNames.Count > 1 ? equipmentNames[1] : "";
                    txtEquipmentName3.Text = equipmentNames.Count > 2 ? equipmentNames[2] : "";

                    // Assign reservation dates to the text blocks
                    txtReservationDate1.Text = reservationDates.Count > 0 ? reservationDates[0] : "";
                    txtReservationDate2.Text = reservationDates.Count > 1 ? reservationDates[1] : "";
                    txtReservationDate3.Text = reservationDates.Count > 2 ? reservationDates[2] : "";
                }
            }


        }
    }
}
