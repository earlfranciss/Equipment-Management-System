using EQMS.Checkout;
using EQMS.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQMS.Checkout
{
    internal class CheckoutViewModel
    {
        private readonly CheckoutDAO CheckoutDAO;
        public readonly dbm db = new dbm();
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable _data;

        public CheckoutViewModel()
        {
            CheckoutDAO = new CheckoutDAO(db.connectionString);
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
            return CheckoutDAO.GetData(CheckoutQuery());
        }


        public DataTable SearchItem(string search)
        {
            return CheckoutDAO.Search(search, FindCheckout());
        }


        private string FindCheckout()
        {
            return "WITH CheckoutSearch AS " +
                "(SELECT c.CheckoutID AS ID, " +
                "e.Name AS Name, " +
                "e.Type AS Type, " +
                "CONVERT(varchar, r.StartDate) AS StartDate, " +
                "CONVERT(varchar, r.EndDate) AS ReturnDate, " +
                "c.Status AS Status, " +
                "CASE WHEN c.ReturnDate IS NOT NULL THEN CONVERT(varchar, c.ReturnDate) " +
                "ELSE 'Not yet returned' END AS CompletionDate " +
                "FROM Checkout c " +
                "LEFT JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                "LEFT JOIN Equipment e ON r.EquipmentID = e.EquipmentID) " +

                "SELECT  ID, " +
                        "Name, " +
                "Type, " +
                "StartDate, " +
                "ReturnDate, " +
                "Status," +
                "CompletionDate " +
                "FROM CheckoutSearch " +
                    "WHERE ID LIKE @SearchText " +
                    "OR Name LIKE @SearchText " +
                    "OR Type LIKE @SearchText " +
                    "OR StartDate LIKE '%' + @SearchText + '%' " +
                    "OR ReturnDate LIKE '%' + @SearchText + '%' " +
                    "OR Status Like @SearchText " +
                    "OR CompletionDate LIKE '%' + @SearchText + '%' ;";
        }

        private string CheckoutQuery()
        {
            return "SELECT c.CheckoutID AS ID, e.Name AS Name, e.Type AS Type, CONVERT(varchar, r.StartDate) AS StartDate, CONVERT(varchar, r.EndDate) AS ReturnDate, c.Status AS Status, " +
                "CASE WHEN c.ReturnDate IS NOT NULL THEN CONVERT(varchar, c.ReturnDate) " + 
                "ELSE 'Not yet returned' END AS CompletionDate " +
                "FROM Checkout c " +
                "JOIN Reservation r ON c.ReservationID = r.ReservationID " +
                "JOIN Equipment e ON r.EquipmentID = e.EquipmentID;";
        }
    }
}
