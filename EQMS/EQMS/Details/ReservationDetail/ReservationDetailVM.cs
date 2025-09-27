using ControlzEx.Standard;
using EQMS.DatabaseManager;
using EQMS.Details.EquipmentDetail;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace EQMS.Details.ReservationDetail
{
    internal class ReservationDetailVM
    {
        private readonly IReservationDetailDA resDA;
        public readonly dbm db = new dbm();
        public int ID { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime BorrowDate { get; set; }
        public TimeSpan BorrowTime { get; set; }
        public DateTime ReturnDate { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Status { get; set; }
        public int EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public string Type { get; set; }
        public int BorrowerID { get; set; }
        public string BorrowerFN { get; set; }
        public string BorrowerLN { get; set; }
        public string Role { get; set; }
        public string Purpose { get; set; }
        public string Subject { get; set; }
        public string Professor { get; set; }


        public ReservationDetailVM()
        {
            resDA = new ReservationDetailDA(db.connectionString);
        }

        public bool SaveEdit(string id, DateTime reservationDate, DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox, string startAmPmComboBox, 
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox, string endAmPmComboBox, 
            string newEquipmentName, string professor, string purpose, string status, string subject)
        {
            if (CheckReservationDate(reservationDate, startDatePicker) && BorrowDateErrorText(startDatePicker).Equals("") 
                && BorrowingHourMinErrorText(startHourTextBox, startMinuteTextBox).Equals("") && ReturnDateErrorText(endDatePicker).Equals("") 
                && ReturnHourMinErrorText(endHourTextBox, endMinuteTextBox).Equals("") && EquipmentErrorText(newEquipmentName).Equals("") 
                && ProfessorErrorText(professor).Equals("") && PurposeErrorText(purpose).Equals("") && SubjectErrorText(subject).Equals("") && StatusErrorText(status).Equals(""))
            {
                return resDA.SaveEdit(id, reservationDate, startDatePicker, startHourTextBox, startMinuteTextBox, startAmPmComboBox, 
                    endDatePicker, endHourTextBox, endMinuteTextBox, endAmPmComboBox, 
                    newEquipmentName, professor, purpose, status, subject, EquipmentName); 
            }
            else
            {
                return false;
            }
        }

        // Load Reservation Info
        public void LoadReservationByID(int userID)
        {
            EQMS.Reservation.Reservation reservation = resDA.GetID(userID);
            if (reservation != null)
            {
                this.ID = reservation.ID;
                this.ReservationDate = reservation.ReservationDate;
                this.BorrowDate = reservation.BorrowDate;
                this.BorrowTime = reservation.BorrowTime;
                this.ReturnDate = reservation.ReturnDate;
                this.ReturnTime = reservation.ReturnTime;
                this.Status = reservation.Status;
                this.EquipmentID = reservation.EquipmentID;
                this.EquipmentName = reservation.EquipmentName;
                this.Type = reservation.Type;
                this.BorrowerID = reservation.BorrowerID;
                this.BorrowerFN = reservation.BorrowerFN;
                this.BorrowerLN = reservation.BorrowerLN;
                this.Role = reservation.Role;
                this.Purpose = reservation.Purpose;
                this.Subject = reservation.Subject;
                this.Professor = reservation.Professor;
            }
        }
        public bool CancelReservation(string id)
        {
            return resDA.CancelReservation(id);
        }

        public bool ProcessCheckout(string id)
        {
            return resDA.ProceedCheckout(id);
        }

        public bool Reserve(DateTime reservationDate, DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox, string startAmPmComboBox,
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox, string endAmPmComboBox,
            string newEquipmentName, string type, string borrowerID, string fn, string ln, string role, string email, string phone,
            string professor, string purpose, string subject)
        {
            if (CheckReservationDate(reservationDate, startDatePicker) && CheckReservationError(startDatePicker, startHourTextBox, startMinuteTextBox,
                 endDatePicker, endHourTextBox, endMinuteTextBox,newEquipmentName, professor, purpose, subject, type) 
                && CheckBorrowerError(borrowerID, fn, ln, role, email, phone))
            {
                return resDA.Reserve(reservationDate, startDatePicker, startHourTextBox, startMinuteTextBox, startAmPmComboBox,
            endDatePicker, endHourTextBox, endMinuteTextBox, endAmPmComboBox,
            newEquipmentName, type, borrowerID, fn, ln, role, email, phone,
            professor, purpose, subject);
            }
            else
            {
                return false;
            }
        }

        //Check if the reservation date is 3 days prior to the borrowing date
        public bool CheckReservationDate(DateTime reservationDate, DateTime borrowingDate)
        {
            TimeSpan difference = borrowingDate - reservationDate;
            int differenceInDays = (int)difference.TotalDays;

            if (differenceInDays >= 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckReservationError(DateTime startDatePicker, string startHourTextBox, string startMinuteTextBox,
            DateTime endDatePicker, string endHourTextBox, string endMinuteTextBox,
            string newEquipmentName, string professor, string purpose, string subject, string type)
        {
            if(BorrowDateErrorText(startDatePicker).Equals("") && TypeErrorText(type).Equals("")
                && BorrowingHourMinErrorText(startHourTextBox, startMinuteTextBox).Equals("") && ReturnDateErrorText(endDatePicker).Equals("")
                && ReturnHourMinErrorText(endHourTextBox, endMinuteTextBox).Equals("") && EquipmentErrorText(newEquipmentName).Equals("")
                && ProfessorErrorText(professor).Equals("") && PurposeErrorText(purpose).Equals("") && SubjectErrorText(subject).Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string ReservationErrorText(DateTime reservationDate, DateTime borrowingDate)
        {
            if(CheckReservationDate(reservationDate, borrowingDate))
            {
                return "";
            }
            else
            {
                return "Reservation date should be 3 days before Borrowing date";
            }
        }
        public string BorrowDateErrorText(DateTime borrowingDate)
        {
            if (borrowingDate == null)
            {
                return " Please select a date.";
            }
            else
            {
                return "";
            }
        }
        public string BorrowingHourMinErrorText(string hour, string min)
        {
            if (hour == "" && min == "")
            {
                return " Please input time.";
            }
            else
            {
                if (int.Parse(hour) <= 12 && int.Parse(min) < 60)
                {
                    return "";
                }
                else
                {
                    return "Please enter correct format";
                }
            }
        }

        public string ReturnHourMinErrorText(string hour, string min)
        {
            if (hour == "" && min == "")
            {
                return " Please input time.";
            }
            else
            {
                if (int.Parse(hour) <= 12 && int.Parse(min) < 60)
                {
                    return "";
                }
                else
                {
                    return "Please enter correct format";
                }
            }
        }

        public string ReturnDateErrorText(DateTime returnDate)
        {
            if (returnDate == null)
            {
                return " Please select a date.";
            }
            else
            {
                return "";
            }
        }

        public string EquipmentErrorText(string name)
        {
            if (name != "")
            {
                if (name.Equals(EquipmentName))
                {
                    return "";
                }
                else
                {
                    if (resDA.CheckEquipment(name))
                    {
                        return "";
                    }
                    else
                    {
                        return "No available " + name;
                    }
                }
                
            }
            else
            {
                return "Please input an equipment";
            }
        }

        public string TypeErrorText(string type)
        {
            if (type == "")
            {
                return "Please enter equipment type";
            }
            else
            {
                return "";
            }
        }


        public string ProfessorErrorText(string professor)
        {
            if (professor == "")
            {
                return "Please enter professor's name";
            }
            else
            {
                return "";
            }
        }
        public string PurposeErrorText(string purpose)
        {
            if (purpose == "")
            {
                return "Please enter purpose";
            }
            else
            {
                return "";
            }
        }

        public string SubjectErrorText(string subject)
        {
            if (subject == "")
            {
                return "Please enter subject";
            }
            else
            {
                return "";
            }
        }

        public string StatusErrorText(string status)
        {
            if (status == "")
            {
                return "Please enter status";
            }
            else
            {
                return "";
            }
        }


        public string IDErrorText(string id)
        {
            if (id == "")
            {
                return "Please enter Borrower ID";
            }
            else
            {
                if (resDA.UnpaidAccounts(id) > 0)
                {
                    return "You still have unpaid accounts";
                }
                else
                {
                    return "";
                }
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
        public string EmailErrorText( string email)
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
        public string PhoneErrorText(string phone)
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


        public bool CheckBorrowerError(string id, string fn, string ln, string role, string email, string phone)
        {
            if (IDErrorText(id).Equals("") && FNErrorText(fn).Equals("") && LNErrorText(ln).Equals("") && RoleErrorText(role).Equals("") && EmailErrorText(email).Equals("") && PhoneErrorText( phone).Equals(""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
