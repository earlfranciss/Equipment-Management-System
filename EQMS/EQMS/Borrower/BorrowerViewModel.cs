using EQMS.Borrower;
using EQMS.DatabaseManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
namespace EQMS.Borrower
{
    internal class BorrowerViewModel : INotifyPropertyChanged 
    {
        private readonly BorrowerDAO BorrowerDAO;
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly dbm db = new dbm();
        private DataTable _data;

        public BorrowerViewModel()
        {
            BorrowerDAO = new BorrowerDAO(db.connectionString);
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
            return BorrowerDAO.GetData(BorrowerQuery());
        }


        public DataTable SearchItem(string search)
        {
            return BorrowerDAO.Search(search, FindBorrower());
        }

        private string FindBorrower()
        {
            return "SELECT  b.BorrowerID AS ID, " +
                        "b.LastName + ', ' + b.FirstName AS Name, " +
                        "b.Type AS Role, " +
                        "b.PhoneNo AS Phone, " +
                        "b.Email AS Email " +
                "FROM Borrower b " +
                    "WHERE BorrowerID LIKE @SearchText " +
                    "OR FirstName LIKE @SearchText " +
                    "OR LastName LIKE @SearchText " +
                    "OR Type LIKE @SearchText " +
                    "OR PhoneNo LIKE @SearchText " +
                    "OR Email Like @SearchText;";


        }
        private string BorrowerQuery()
        {
            return "SELECT b.BorrowerID AS ID, b.LastName + ', ' + b.FirstName AS Name, b.Type AS Role, b.PhoneNo AS Phone, b.Email AS Email " +
                "FROM Borrower b ";
        }
    }
}
