using ControlzEx.Standard;
using EQMS.DatabaseManager;
using EQMS.Equipment;
using EQMS.Reservation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EQMS.Reservation
{
    internal class ReservationViewModel
    {
        private readonly ReservationDAO ReservationDAO;
        public readonly dbm db = new dbm();
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable _data;

        public ReservationViewModel()
        {
            ReservationDAO = new ReservationDAO(db.connectionString);
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
            return ReservationDAO.GetData(ReservationQuery());
        }

        public DataTable SearchItem(string search)
        {
            return ReservationDAO.Search(search, FindReservation());
        }


        private string FindReservation()
        {
            return "SELECT r.ReservationID AS ID, " +
                "e.Name AS EquipmentName, " +
                "e.Type AS Type, " +
                "b.LastName + ', ' + b.FirstName AS BorrowerName, " +
                "CONVERT(varchar, r.ReservationDate) AS ReservationDate, " +
                "CONVERT(varchar, r.StartDate) AS StartDate, " +
                "CONVERT(varchar, r.EndDate) AS ReturnDate, " +
                "r.Status AS Status " +
                "FROM Reservation r " +
                "LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                "LEFT JOIN Borrower b ON r.BorrowerID = b.BorrowerID " +
                "LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID " +
                    "WHERE (r.ReservationID LIKE @SearchText " +
                    "OR e.Name LIKE @SearchText " +
                    "OR e.Type LIKE @SearchText " +
                    "OR b.FirstName LIKE @SearchText " +
                    "OR b.LastName LIKE @SearchText " +
                    "OR CONVERT(varchar, r.ReservationDate) LIKE '%' + @SearchText + '%' " +
                    "OR CONVERT(varchar, r.StartDate) LIKE '%' + @SearchText + '%' " +
                    "OR CONVERT(varchar, r.EndDate) LIKE '%' + @SearchText + '%' " +
                    "OR r.Status Like @SearchText) " +
                    "AND c.ReservationID IS NULL;";
        }

        private string ReservationQuery()
        {
            return "SELECT r.ReservationID AS ID, e.Name AS EquipmentName, b.LastName + ', ' + b.FirstName AS BorrowerName, e.Type AS Type, " +
                "CONVERT(varchar, r.ReservationDate) AS ReservationDate, " +
                "CONVERT(varchar, r.StartDate) AS StartDate, CONVERT(varchar, r.EndDate) AS ReturnDate, r.Status AS Status " +
                "FROM Reservation r " +
                "JOIN Equipment e ON r.EquipmentID = e.EquipmentID " +
                "JOIN Borrower b ON r.BorrowerID = b.BorrowerID " +
                "LEFT JOIN Checkout c ON r.ReservationID = c.ReservationID " +
                "WHERE c.ReservationID IS NULL;";
        }
    }
}
