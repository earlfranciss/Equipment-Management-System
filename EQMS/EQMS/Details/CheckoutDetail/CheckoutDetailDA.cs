using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EQMS.Details.CheckoutDetail
{
    public interface ICheckoutDetailDA
    {
        string _connectionString { get; }
        SqlConnection GetConnection();
        bool SaveEdit(string id, DateTime completiondate, string status, string condition, string description, double payment, string paymentStatus);
        EQMS.Checkout.Checkout GetID(int user);
    }
    internal class CheckoutDetailDA : ICheckoutDetailDA
    {
        public string _connectionString { get; }

        public CheckoutDetailDA(string _connectionString)
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


        // Get Employee Info using ID
        public EQMS.Checkout.Checkout GetID(int user)
        {
            using (SqlConnection connection = GetConnection())
            {
                EQMS.Checkout.Checkout checkout = null;

                string query = "SELECT c.CheckoutID, " +
                    "c.Status AS Status, " +
                    "CASE WHEN c.ReturnDate IS NOT NULL THEN c.ReturnDate ELSE '1900-01-01' END AS CompletionDate, " +
                    "c.ReturnCondition AS Condition, " +
                    "c.Description AS Description, " +
                    "c.Cost AS Cost, " +
                    "c.PaymentStatus AS PaymentStatus, " +
                    "DATEFROMPARTS(YEAR(r.ReservationDate), MONTH(r.ReservationDate), DAY(r.ReservationDate)) AS ReservationDate, " +
                    "DATEFROMPARTS(YEAR(r.StartDate), MONTH(r.StartDate), DAY(r.StartDate)) AS BorrowDate,  " +
                    "CAST(r.StartDate AS time) AS BorrowTime, " +
                    "DATEFROMPARTS(YEAR(r.EndDate), MONTH(r.EndDate), DAY(r.EndDate)) AS ReturnDate, " +
                    "CAST(r.EndDate AS time) AS ReturnTime, " +
                    "r.Professor AS Professor, r.Purpose AS Purpose, r.Subject AS Subject, " +
                    "r.EquipmentID AS EquipmentID, e.Name AS Name, e.Type AS Type, " +
                    "r.BorrowerID AS BorrowerID, b.FirstName AS FirstName, b.LastName AS LastName, b.Type AS Role " +

                    "FROM Checkout c " +
                    "LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                    "LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                    "LEFT JOIN Borrower b ON r.BorrowerID = b.BorrowerID " +
                    "WHERE c.CheckoutID = @CheckoutID;";


                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CheckoutID", user);


                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        checkout = new EQMS.Checkout.Checkout
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
                            Role = (string)reader["Role"],
                            DateReturned = (DateTime)reader["CompletionDate"],
                            Condition = reader["Condition"] != DBNull.Value ? (string)reader["Condition"] : "No condition specified",
                            Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : "No description",
                            Cost = reader["Cost"] != DBNull.Value ? new SqlMoney((decimal)reader["Cost"]) : SqlMoney.Zero,
                            PaymentStatus = reader["Cost"] == DBNull.Value || Convert.ToDouble(reader["Cost"]) == 0.00 ? "Paid" : "Unpaid"
                        };
                        
                        return checkout;
                        
                    }
                    else
                    {
                        MessageBox.Show("wala Niosulod");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("wla asdfqwe Niosulod");
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
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

        // Save Edits to Checkout
        public bool SaveEdit(string id, DateTime completiondate, string status, string condition, string description, double payment, string paymentStatus)
        {

            using (SqlConnection connection = GetConnection())
            {
                string query = "UPDATE Checkout SET " +
                    "ReturnDate = @ReturnDate, " +
                    "Status = @Status, " +
                    "ReturnCondition = @Condition, " +
                    "Description = @Description, " +
                    "Cost = @Cost, " +
                    "PaymentStatus = @PaymentStatus " +
                    "WHERE CheckoutID = @CheckoutID;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CheckoutID", id);
                command.Parameters.AddWithValue("@ReturnDate", completiondate);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@Condition", condition);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Cost", payment);
                command.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

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

    }
}
