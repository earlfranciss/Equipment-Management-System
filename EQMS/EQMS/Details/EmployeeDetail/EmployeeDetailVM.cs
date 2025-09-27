using EQMS.Authentication.Register;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EQMS.DatabaseManager;

namespace EQMS.Details.EmployeeDetail
{
    internal class EmployeeDetailVM
    {
        private readonly IEmployeeDetailDA empDA;
        public readonly dbm db = new dbm();
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }


        public EmployeeDetailVM()
        {
            empDA = new EmployeeDetailDA(db.connectionString);
        }

        public bool SaveEdit(string id, string fn, string ln, string email, string phone, string position)
        {
            if (FNErrorText(fn).Equals("") && LNErrorText(ln).Equals("") && PositionErrorText(position).Equals("") && EmailErrorText(id, email).Equals("") && PhoneErrorText(id, phone).Equals(""))
            {
                return empDA.SaveEdit(id, fn, ln, email, phone, position);
            }
            else
            {
                return false;
            }
        }
        public bool DeleteEmployee(string id)
        {
            return empDA.DeleteEmployee(id);
        }

        // Load Employee Info
        public void LoadEmployeeByID(int userID)
        {
            EQMS.Employee.Employee employee = empDA.GetID(userID);
            if (employee != null)
            {
                this.ID = employee.ID;
                FirstName = employee.FirstName;
                LastName = employee.LastName;
                Email = employee.Email;
                Phone = employee.Phone;
                Position = employee.Position;
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

                empDA.SaveImageToDatabase(id, imageData);
            }
        }


        // Loading Employee Image to Image source
        public BitmapImage SetImage(int id)
        {
            BitmapImage image = empDA.GetImage(id);
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
        public string PositionErrorText(string position)
        {
            if (position == "")
            {
                return "Please enter position";
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
                if (!empDA.EmailExist(id, email))
                {
                    return "";
                }
                else
                {
                    return "Email already exist.";
                }
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
                if (!empDA.PhoneExist(id, phone))
                {
                    return "";
                }
                else
                {
                    return "Phone number already exist.";
                }
            }
            else
            {
                return "Please enter phone number.";
            }
        }


    }
}
