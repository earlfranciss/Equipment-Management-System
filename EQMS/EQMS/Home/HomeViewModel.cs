using EQMS.Borrower;
using EQMS.DatabaseManager;
using EQMS.Equipment;
using EQMS.Reservation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Shapes;

namespace EQMS.Home
{

    internal class HomeViewModel : INotifyPropertyChanged
    {
        private readonly HomeDAO homeDAO;
        public readonly dbm db = new dbm();
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable _data;

        public HomeViewModel()
        {
            homeDAO = new HomeDAO(db.connectionString);
        }

        public DataTable Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DataTable GetDataFromDatabase()
        {
            return homeDAO.MergedData(homeDAO.GetData(ReservationQuery()), homeDAO.GetData(CheckoutQuery()));
        }

        public DataTable SearchItem(string search)
        {
            return homeDAO.Search(search, FindItem());
        }


        private string FindItem()
        {
            return "WITH CombinedSearch AS ( " +
                "SELECT c.CheckoutID AS ID, e.Name AS Name, e.Type AS Type, " +
                    "CONVERT(varchar, r.StartDate) AS StartDate, " +
                    "CONVERT(varchar, r.EndDate) AS ReturnDate, " +
                    "'Checked-Out' AS State, c.Status AS Status " +
                "FROM Checkout c " +
                    "LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                    "LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                    
                "UNION ALL " +

                "SELECT r.ReservationID AS ID, e.Name AS EquipmentName, e.Type AS Type, " +
                    "CONVERT(varchar, r.StartDate) AS StartDate, " +
                    "CONVERT(varchar, r.EndDate) AS ReturnDate, " +
                    "CASE WHEN c.ReservationID IS NOT NULL THEN 'Checked-Out' ELSE 'Reserved' END AS State, r.Status AS Status " +
                "FROM Reservation r " +
                "LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                "LEFT JOIN Borrower b ON r.BorrowerID = b.BorrowerID " +
                "LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID " +
                "WHERE c.ReservationID IS NULL) " +

                "SELECT ID, Name, Type, StartDate, ReturnDate, State, Status " +
                "FROM CombinedSearch " +
                "WHERE ID LIKE @SearchText " +
                    "OR Name LIKE @SearchText " +
                    "OR Type LIKE @SearchText " +
                    "OR StartDate LIKE '%' + @SearchText + '%' " +
                    "OR ReturnDate LIKE '%' + @SearchText + '%' " +
                    "OR State LIKE @SearchText " +
                    "OR Status LIKE @SearchText;";
        }

        private string ReservationQuery()
        {
            return "SELECT r.ReservationID AS ID, e.Name AS Name, e.Type AS Type, " +
                "CONVERT(varchar, r.StartDate) AS StartDate, CONVERT(varchar, r.EndDate) AS ReturnDate, 'Reserved' AS State, r.Status AS Status  " +
                "FROM Reservation r " +
                "JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                "LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID " +
                "WHERE c.ReservationID IS NULL;";
        }
        private string CheckoutQuery()
        {
            return "SELECT c.CheckoutID AS ID, e.Name AS Name, e.Type AS Type, CONVERT(varchar, r.StartDate) AS StartDate, CONVERT(varchar, r.EndDate) AS ReturnDate, 'Checked-out' AS State, c.Status AS Status " +
                "FROM Checkout c " +
                "JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                "JOIN Equipment e ON r.EquipmentID = e.EquipmentID;";
        }
    }
}
