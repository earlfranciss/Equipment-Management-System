using EQMS.DatabaseManager;
using EQMS.Details.ReservationDetail;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQMS.Details.CheckoutDetail
{
    internal class CheckoutDetailVM
    {
        private readonly ICheckoutDetailDA chkDA;
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
        public string Description { get; set; }
        public string Condition { get; set; }
        public SqlMoney Cost { get; set; }
        public string Professor { get; set; }
        public DateTime DateReturned { get; set; }
        public string PaymentStatus { get; set; }


        public CheckoutDetailVM()
        {
            chkDA = new CheckoutDetailDA(db.connectionString);
        }

        public bool SaveEdit(string id, DateTime completiondate, string status, string condition, string description, SqlMoney payment,
            string paymentStatus)
        {
            condition = condition == null ? "Okay" : "Needs attention";
            double paymentAmount = payment.ToDouble();
            paymentAmount = payment.IsNull || paymentAmount == 0.00? Convert.ToDouble(0) : paymentAmount;
            paymentStatus = payment.IsNull || paymentAmount == 0 ? "Paid" : "Unpaid";

            if (ReturnErrorText(completiondate).Equals("") && StatusErrorText(status).Equals(""))
            {
                return chkDA.SaveEdit(id, completiondate, status, condition, description, paymentAmount, paymentStatus);
            }
            else
            {
                return false;
            }
        }

        // Load Reservation Info
        public void LoadCheckoutByID(int userID)
        {
            EQMS.Checkout.Checkout reservation = chkDA.GetID(userID);
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
                this.Description = reservation.Description;
                this.Condition =    reservation.Condition;
                this.Cost = reservation.Cost;
                this.DateReturned = reservation.DateReturned;
                this.PaymentStatus = reservation.PaymentStatus;
            }
        }


        public string ReturnErrorText(DateTime completionDate)
        {
            if (completionDate != null)
            {
                return "";
            }
            else
            {
                return "Please select a date.";
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

    }
}
