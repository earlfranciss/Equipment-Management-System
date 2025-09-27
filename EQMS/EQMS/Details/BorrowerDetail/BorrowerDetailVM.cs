
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EQMS.DatabaseManager;


namespace EQMS.Details.BorrowerDetail
{
    internal class BorrowerDetailVM
    {
        private readonly IBorrowerDetailDA brwrDA;
        public readonly dbm db = new dbm();
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }


        public BorrowerDetailVM()
        {
            brwrDA = new BorrowerDetailDA(db.connectionString);
        }

        public bool SaveEdit(string id, string fn, string ln, string email, string phone, string role)
        {
            if (FNErrorText(fn).Equals("") && LNErrorText(ln).Equals("") && RoleErrorText(role).Equals("") && EmailErrorText(id, email).Equals("") && PhoneErrorText(id, phone).Equals(""))
            {
                return brwrDA.SaveEdit(id, fn, ln, email, phone, role);
            }
            else
            {
                return false;
            }
        }
        public bool DeleteBorrower(string id)
        {
            return brwrDA.DeleteBorrower(id);
        }

        // Load Borrower Info
        public void LoadBorrowerByID(int userID)
        {
            EQMS.Borrower.Borrower borrower = brwrDA.GetID(userID);
            if (borrower != null)
            {
                this.ID = borrower.ID;
                FirstName = borrower.FirstName;
                LastName = borrower.LastName;
                Email = borrower.Email;
                Phone = borrower.Phone;
                Role = borrower.Role;
            }
        }


        //Browse Image 
        public void BrowseImage(int id)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg; *.jpeg; *.png; *.gif";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {

                string filePath = openFileDialog.FileName;
                byte[] imageData = File.ReadAllBytes(filePath);

                brwrDA.SaveImageToDatabase(id, imageData);
            }
        }


        // Loading Borrower Image to Image source
        public BitmapImage SetImage(int id)
        {
            BitmapImage image = brwrDA.GetImage(id);
            if (image != null)
            {
                return image;
            }
            else
            {
                return null;
            }
        }

        public string FNErrorText(string fn)
        {
            if (fn == "")
            {
                return "Please enter first name";
            }
            else
            {
                return "";
            }
        }
        public string LNErrorText(string ln)
        {
            if (ln == "")
            {
                return "Please enter last name";
            }
            else
            {
                return "";
            }
        }
        public string RoleErrorText(string role)
        {
            if (role == "")
            {
                return "Please enter role";
            }
            else
            {
                return "";
            }
        }
        public string EmailErrorText(string id, string email)
        {
            if (email != "")
            {
                
                return "";
            }
            else
            {
                return "Please enter email.";
            }
        }
        public string PhoneErrorText(string id, string phone)
        {
            if (phone != "")
            {
                return "";
            }
            else
            {
                return "Please enter phone number.";
            }
        }
    }
}
