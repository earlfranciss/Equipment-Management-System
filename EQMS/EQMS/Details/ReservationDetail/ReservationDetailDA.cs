using ControlzEx.Standard;
using EQMS.Borrower;
using EQMS.Equipment;
using EQMS.Reservation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace EQMS.Details.ReservationDetail
{
    public interface IReservationDetailDA
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        bool SaveEdit(string id, DateTime reservationDate, DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox, string startAmPmComboBox, 
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox, string endAmPmComboBox, 
            string newEquipmentName, string professor, string purpose, string status, string subject, string currentEquipment);
        void AddBorrower(string id, string fn, string ln, string email, string phone, string role);
        bool Reserve(DateTime reservationDate, DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox, string startAmPmComboBox,
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox, string endAmPmComboBox,
            string newEquipmentName, string type, string borrowerID, string fn, string ln, string role, string email, string phone,
            string professor, string purpose, string subject);
        bool CancelReservation(string id);
        bool ProceedCheckout(string id);
        bool CheckEquipment(string name);
        int UnpaidAccounts(string id);
        EQMS.Reservation.Reservation GetID(int user);
    }

    internal class ReservationDetailDA : IReservationDetailDA
    {
        public string _connectionString { get; }

        public ReservationDetailDA(string _connectionString)
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

        private DateTime GetSelectedDateTime(DateTime datePicker, string hourTextBox, string minuteTextBox, string amPmComboBox)
        {
            DateTime selectedDate = datePicker;

            int hour = int.Parse(hourTextBox);
            int minute = int.Parse(minuteTextBox);

            if (amPmComboBox != null && amPmComboBox == "pm" && hour < 12)
            {
                hour += 12;
            }
            else if (amPmComboBox != null && amPmComboBox == "am" && hour == 12)
            {
                hour = 0;
            }

            return new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
        }

        

        // Save Edits to Reservation
        public bool SaveEdit(string id, DateTime reservationDate, DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox, string startAmPmComboBox, 
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox, string endAmPmComboBox, 
            string newEquipmentName, string professor, string purpose, string status, string subject, string currentEquipment)
        {
            
            DateTime startDate = GetSelectedDateTime(startDatePicker, startHourTextBox, startMinuteTextBox, startAmPmComboBox);
            DateTime endDate = GetSelectedDateTime(endDatePicker, endHourTextBox, endMinuteTextBox, endAmPmComboBox);

            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Reservation SET " +
                    "ReservationDate = @ReservationDate, " +
                    "StartDate = @StartDate, " +
                    "EndDate = @EndDate, " +
                    "EquipmentID = CASE " +
                        "WHEN @NewEquipmentName = @CurrentEquipmentName THEN Reservation.EquipmentID " +
                        "ELSE ( SELECT TOP 1 e.EquipmentID " +
                            "FROM Equipment e " +
                            "WHERE e.Name = @NewEquipmentName " +
                            "AND e.EquipmentID NOT IN ( " +
                            "SELECT r.EquipmentID FROM Checkout c  " +
                            "LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID  " +
                            "WHERE c.Status <> 'Completed' OR r.Status <> 'Completed' ) " +
                            "ORDER BY e.EquipmentID) " +
                        "END, " +
                    "Professor = @Professor, " +
                    "Purpose = @Purpose, " +
                    "Status = @Status, " +
                    "Subject = @Subject " +
                    "WHERE ReservationID = @ReservationID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", id);
                command.Parameters.AddWithValue("@ReservationDate", reservationDate);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                command.Parameters.AddWithValue("@NewEquipmentName", newEquipmentName);
                command.Parameters.AddWithValue("@CurrentEquipmentName", currentEquipment);
                command.Parameters.AddWithValue("@Professor", professor);
                command.Parameters.AddWithValue("@Purpose", purpose);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@Subject", subject);

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


        public void AddBorrower(string id, string fn, string ln, string email, string phone, string role)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "INSERT INTO Borrower (BorrowerID, FirstName, LastName, PhoneNo, Email, Type) VALUES (@BorrowerID, @FirstName, @LastName, @Phone, @Email, @Role);";
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public bool FindBorrower(string id)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "SELECT COUNT(1) FROM Borrower WHERE BorrowerID = @BorrowerID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BorrowerID", id);

                try
                { 
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;

                }
            }
        }

        // Save Edits to Reservation
        public bool Reserve(DateTime reservationDate, DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox, string startAmPmComboBox,
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox, string endAmPmComboBox,
            string newEquipmentName, string type, string borrowerID, string fn, string ln, string role, string email, string phone,
            string professor, string purpose, string subject)
        {

            if (!FindBorrower(borrowerID))
            {
                AddBorrower(borrowerID, fn, ln, email, phone, role);
            }

            DateTime startDate = GetSelectedDateTime(startDatePicker, startHourTextBox, startMinuteTextBox, startAmPmComboBox);
            DateTime endDate = GetSelectedDateTime(endDatePicker, endHourTextBox, endMinuteTextBox, endAmPmComboBox);

            using (SqlConnection connection = GetConnection())
            {
                string query = "INSERT INTO Reservation (ReservationDate, StartDate, EndDate, EquipmentID, BorrowerID, Professor, Purpose, Status, Subject) " +
                                    "SELECT @ReservationDate, @StartDate, @EndDate, " +
                                    "(SELECT TOP 1 e.EquipmentID " +
                                        "FROM Equipment e " +
                                        "WHERE e.Name = @NewEquipmentName " +
                                        "AND e.EquipmentID NOT IN ( " +
                                        "SELECT r.EquipmentID FROM Checkout c  " +
                                        "LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID  " +
                                        "WHERE c.Status <> 'Completed' OR r.Status <> 'Completed' ) " +
                                        "ORDER BY e.EquipmentID), " +
                                    "@BorrowerID, @Professor, @Purpose, 'Booked', @Subject;";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationDate", reservationDate);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                command.Parameters.AddWithValue("@NewEquipmentName", newEquipmentName);
                command.Parameters.AddWithValue("@BorrowerID", borrowerID);
                command.Parameters.AddWithValue("@Professor", professor);
                command.Parameters.AddWithValue("@Purpose", purpose);
                command.Parameters.AddWithValue("@Subject", subject);

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


        // Get Employee Info using ID
        public EQMS.Reservation.Reservation GetID(int user)
        {
            using (SqlConnection connection = GetConnection())
            {
                EQMS.Reservation.Reservation reservation = null;

                string query = "SELECT r.ReservationID, " +
                    "DATEFROMPARTS(YEAR(r.ReservationDate), MONTH(r.ReservationDate), DAY(r.ReservationDate)) AS ReservationDate, " +
                    "DATEFROMPARTS(YEAR(r.StartDate), MONTH(r.StartDate), DAY(r.StartDate)) AS BorrowDate,  " +
                    "CAST(r.StartDate AS time) AS BorrowTime, " +
                    "DATEFROMPARTS(YEAR(r.EndDate), MONTH(r.EndDate), DAY(r.EndDate)) AS ReturnDate, " +
                    "CAST(r.EndDate AS time) AS ReturnTime, " +
                    "r.Professor AS Professor, r.Purpose AS Purpose, r.Status AS Status, r.Subject AS Subject, " +
                    "r.EquipmentID AS EquipmentID, e.Name AS Name, e.Type AS Type, " +
                    "r.BorrowerID AS BorrowerID, b.FirstName AS FirstName, b.LastName AS LastName, b.Type AS Role " +
                    "FROM Reservation r " +
                    "JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                    "JOIN Borrower b ON r.BorrowerID = b.BorrowerID " +
                    "WHERE r.ReservationID = @ReservationID;";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", user);


                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        reservation = new EQMS.Reservation.Reservation
                        {
                            ID = user,
                            ReservationDate = ((DateTime)reader["ReservationDate"]).Date,
                            BorrowDate = (DateTime)reader["BorrowDate"],
                            BorrowTime = (TimeSpan)reader["BorrowTime"],
                            ReturnDate = (DateTime)reader["ReturnDate"],
                            ReturnTime = (TimeSpan)reader["ReturnTime"],
                            Professor = (string)reader["Professor"],
                            Purpose = (string)reader["Purpose"],
                            Status = (string)reader["Status"],
                            Subject = (string)reader["Subject"],
                            EquipmentID = (int)reader["EquipmentID"],
                            EquipmentName = (string)reader["Name"],
                            Type = (string)reader["Type"],
                            BorrowerID = (int)reader["BorrowerID"],
                            BorrowerFN = (string)reader["FirstName"],
                            BorrowerLN = (string)reader["LastName"],
                            Role = (string)reader["Role"]
                        };
                        return reservation;
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


        public int UnpaidAccounts(string id)
        {
            int count = 0;

            string query = @"
                            SELECT 
                                CASE 
                                    WHEN EXISTS (
                                        SELECT 1 
                                        FROM Checkout c2
                                        WHERE r.ReservationID = c2.ReservationID AND c2.PaymentStatus = 'Unpaid'
                                    ) THEN 'True'
                                    ELSE 'False'
                                END AS PaymentAccount
                            FROM 
                                Reservation r
                                LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID
                            WHERE r.BorrowerID = @BorrowerID;";

            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BorrowerID", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string paymentAccount = reader.GetString(reader.GetOrdinal("PaymentAccount"));
                        if (paymentAccount == "True")
                        {
                            count++;
                        }
                    }
                }
            }

            return count;


        }
        public bool CheckEquipment(string name)
        {
            using (SqlConnection connection = GetConnection())
            {
                bool isAvailable = false;
                string query = "SELECT CASE" +
                    " WHEN EXISTS(" +
                    " SELECT 1 " +
                    " FROM Equipment e" +
                    " WHERE e.Name = @Name " +
                    "  AND e.EquipmentID NOT IN( " +
                    "SELECT r.EquipmentID " +
                    " FROM Checkout c " +
                    " LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                    " WHERE c.Status<> 'Completed' OR r.Status<> 'Completed' ) )" +
                    "THEN 'true'  ELSE 'false' END AS IsEquipmentAvailable;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);

                    
                try
                {
                    string result = command.ExecuteScalar().ToString();

                    isAvailable = bool.Parse(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return isAvailable;
            }
        }

        public bool CancelReservation(string id)
        {
            using (SqlConnection connection = GetConnection())
            {

                string query = "UPDATE Reservation SET Status = 'Cancelled' WHERE ReservationID = @ReservationID;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", id);

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

        public bool ProceedCheckout(string id)
        {
            using (SqlConnection connection = GetConnection())
            {
                string query = "DECLARE @NewID INT; " +
                    "SELECT @NewID = ISNULL(MAX(CheckoutID) + 1, 1) FROM Checkout; " +
                    "INSERT INTO Checkout (CheckoutID, ReservationID, Status) VALUES (@NewID, @ReservationID, 'Open');";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationID", id);

                string query1 = "update Reservation set Status = 'Completed' where ReservationID = @ReservationID;";
                SqlCommand command1 = new SqlCommand(query1, connection);
                command1.Parameters.AddWithValue("@ReservationID", id);

                try
                {
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                    return true;
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
