using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQMS.DatabaseManager;
using EQMS.Equipment;

namespace EQMS.Employee
{
    internal class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeDAO EmployeeDAO;
        public readonly dbm db = new dbm();
        public event PropertyChangedEventHandler PropertyChanged;
        private DataTable _data;

        public EmployeeViewModel()
        {
            EmployeeDAO = new EmployeeDAO(db.connectionString);
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
            return EmployeeDAO.GetData(EmployeeQuery());
        }

        public DataTable SearchItem(string search)
        {
            return EmployeeDAO.Search(search, FindEmployee());
        }

        private string FindEmployee()
        {
            return "SELECT  e.EmployeeID AS ID, " +
                        "e.LastName + ', ' + e.FirstName AS Name, " +
                        "e.Position AS Position, " +
                        "e.PhoneNo AS Phone, " +
                        "e.Email AS Email " +
                "FROM Employee e " +
                    "WHERE EmployeeID LIKE @SearchText " +
                    "OR FirstName LIKE @SearchText " +
                    "OR LastName LIKE @SearchText " +
                    "OR Position LIKE @SearchText " +
                    "OR PhoneNo LIKE @SearchText " +
                    "OR Email Like @SearchText;";

        }
        private string EmployeeQuery()
        {
            return "SELECT e.EmployeeID AS ID, e.LastName + ', ' + e.FirstName AS Name, e.Position AS Position, e.PhoneNo AS Phone, e.Email AS Email " +
                "FROM Employee e ";
        }
    }
}
