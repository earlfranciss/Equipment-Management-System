using EQMS.DatabaseManager;
using EQMS.Equipment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EQMS.Equipment
{
    internal class EquipmentViewModel : INotifyPropertyChanged
    {
        private readonly EquipmentDAO EquipmentDAO;
        public readonly dbm db = new dbm();
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable _data;

        public EquipmentViewModel()
        {
            EquipmentDAO = new EquipmentDAO(db.connectionString);
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
            return EquipmentDAO.GetData(EquipmentQuery());
        }

        public DataTable SearchItem(string search)
        {
            return EquipmentDAO.Search(search, FindEquipment());
        }

        private string EquipmentQuery()
        {
            return "SELECT \r\n    e.EquipmentID AS ID, \r\n    e.Name AS Name, \r\n    e.Type AS Type, \r\n    CASE \r\n        WHEN MAX(c.Status) = 'Completed' THEN 'Available' \r\n        WHEN MAX(c.CheckoutID) IS NOT NULL THEN 'Checked-out' \r\n        WHEN MAX(r.ReservationID) IS NOT NULL THEN 'Reserved' \r\n        ELSE 'Available' \r\n    END AS State, \r\n    CASE \r\n        WHEN MAX(c.Status) = 'Completed' THEN 'Available' \r\n        WHEN MAX(c.CheckoutID) IS NOT NULL THEN MAX(c.Status) \r\n        WHEN MAX(r.ReservationID) IS NOT NULL THEN MAX(r.Status) \r\n        ELSE 'Available' \r\n    END AS Status\r\nFROM \r\n    Equipment e \r\nLEFT JOIN \r\n    Reservation r ON e.EquipmentID = r.EquipmentID \r\nLEFT JOIN \r\n    Checkout c ON r.ReservationID = c.ReservationID\r\nGROUP BY \r\n    e.EquipmentID, \r\n    e.Name, \r\n    e.Type;\r\n";
        }

        private string FindEquipment()
        {
            return "WITH EquipmentAvailability AS (\r\n    SELECT \r\n        e.EquipmentID, \r\n        e.Name, \r\n        e.Type, \r\n        CASE \r\n            WHEN c.CheckoutID IS NOT NULL THEN 'Checked-Out' \r\n            WHEN r.ReservationID IS NOT NULL THEN 'Reserved' \r\n            ELSE 'Available' \r\n        END AS State, \r\n        CASE \r\n            WHEN c.CheckoutID IS NOT NULL THEN c.Status \r\n            WHEN r.ReservationID IS NOT NULL THEN r.Status \r\n            ELSE 'Available' \r\n        END AS Status,\r\n        ROW_NUMBER() OVER (PARTITION BY e.EquipmentID ORDER BY \r\n            CASE \r\n                WHEN c.CheckoutID IS NOT NULL THEN 1 \r\n                WHEN r.ReservationID IS NOT NULL THEN 2 \r\n                ELSE 3 \r\n            END) AS RowNum\r\n    FROM \r\n        Equipment e \r\n    LEFT JOIN \r\n        Reservation r ON e.EquipmentID = r.EquipmentID \r\n    LEFT JOIN \r\n        Checkout c ON r.ReservationID = c.ReservationID\r\n)\r\nSELECT \r\n    EquipmentID AS ID, \r\n    Name, \r\n    Type, \r\n    Status, \r\n    State \r\nFROM \r\n    EquipmentAvailability \r\nWHERE \r\n    RowNum = 1 AND\r\n    (EquipmentID LIKE @SearchText OR \r\n    Name LIKE @SearchText OR \r\n    Type LIKE @SearchText OR \r\n    State LIKE @SearchText OR \r\n    Status LIKE @SearchText);\r\n";
        }

    }
}
