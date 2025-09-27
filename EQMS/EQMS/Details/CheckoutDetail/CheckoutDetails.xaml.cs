using EQMS.Details.EmployeeDetail;
using EQMS.Reservation;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EQMS.Details.CheckoutDetail
{
    /// <summary>
    /// Interaction logic for CheckoutDetails.xaml
    /// </summary>
    public partial class CheckoutDetails : Page
    {
        CheckoutDetailVM chkVM = new CheckoutDetailVM();
        DetailWindow DetailWindow = new DetailWindow();
        public event EventHandler DataSaved;

        public CheckoutDetails()
        {
            InitializeComponent();
        }
        public CheckoutDetails(int id)
        {
            InitializeComponent();
            LoadUser(id);
        }

        public void LoadUser(int id)
        {
            chkVM.LoadCheckoutByID(id);
            CheckoutIDtxt.Text = id.ToString();
            Statustxt.Text = chkVM.Status;
            ReservationDatetxt.Text = chkVM.ReservationDate.Date.ToString("yyyy-MM-dd");
            EquipmentIDtxt.Text = chkVM.EquipmentID.ToString();
            EquipmentNametxt.Text = chkVM.EquipmentName;
            EquipmentTypetxt.Text = chkVM.Type;
            BorrowerIDtxt.Text = chkVM.BorrowerID.ToString();
            BorrowerNametxt.Text = chkVM.BorrowerLN + ", " + chkVM.BorrowerFN;
            BorrowerRoletxt.Text = chkVM.Role;
            Purposetxt.Text = chkVM.Purpose;
            Subjecttxt.Text = chkVM.Subject;
            Professortxt.Text = chkVM.Professor;

            editCheckoutIDtxt.Text = id.ToString();
            editStatustext.SelectedItem = chkVM.Status;
            BorrowDate.SelectedDate = chkVM.BorrowDate.Date;
            BorrowHour.Text = (chkVM.BorrowTime.Hours > 12) ? (chkVM.BorrowTime.Hours - 12).ToString() : (chkVM.BorrowTime.Hours).ToString();
            BorrowMin.Text = (chkVM.BorrowTime.Minutes > 1) ? (chkVM.BorrowTime.Minutes).ToString() : "00";
            BorrowAmPm.SelectedIndex = (chkVM.BorrowTime.Hours >= 12 && chkVM.BorrowTime.Minutes > 0) ? 1 : 0;
            ReturnDate.SelectedDate = chkVM.ReturnDate.Date;
            ReturnHour.Text = (chkVM.ReturnTime.Hours > 12) ? (chkVM.ReturnTime.Hours - 12).ToString() : (chkVM.ReturnTime.Hours).ToString();
            ReturnMin.Text = (chkVM.ReturnTime.Minutes > 1) ? (chkVM.ReturnTime.Minutes).ToString() : "00";
            ReturnAmPm.SelectedIndex = (chkVM.ReturnTime.Hours >= 12 && chkVM.ReturnTime.Minutes > 0) ? 1 : 0;
            editEquipmentIDtxt.Text = chkVM.EquipmentID.ToString();
            editEquipmentNametxt.Text = chkVM.EquipmentName;
            editEquipmentTypetxt.Text = chkVM.Type;
            SetCompletionDate();
            editConditiontext.Text = string.IsNullOrEmpty(chkVM.Condition) ? "" : chkVM.Condition.ToString();
            editPaymenttext.Text = (chkVM.Cost.IsNull) ? "0" : chkVM.Cost.ToString();
            editDescriptiontext.Text = string.IsNullOrEmpty(chkVM.Description) ? "" : chkVM.Description.ToString();
            editPaymentStatustext.SelectedItem = string.IsNullOrEmpty(chkVM.PaymentStatus) ? "" : chkVM.PaymentStatus.ToString();
            string BorrowAMPM = (chkVM.BorrowTime.Hours > 12) ? "pm" : "am";
            string ReturnAMPM = (chkVM.ReturnTime.Hours > 12) ? "pm" : "am";
            BorrowDatetxt.Text =  chkVM.BorrowDate.Date.ToString("yyyy-MM-dd") + "  " + BorrowHour.Text + ":" + BorrowMin.Text + " " + BorrowAMPM;
            ReturnDatetxt.Text = chkVM.ReturnDate.Date.ToString("yyyy-MM-dd") + "  " + ReturnHour.Text + ":" + ReturnMin.Text + " " + ReturnAMPM;
        }

        private void SetCompletionDate()
        {
            if (chkVM.DateReturned.Date.ToString("yyyy-MM-dd").Equals("1900-01-01"))
            {
                CompletionDate.SelectedDate = DateTime.Today;
            }
            else
            {
                CompletionDate.Text = chkVM.DateReturned.Date.ToString("yyyy-MM-dd");
            }
        }
        //Text Clear
        private void TextClear()
        {
            editStatustext.SelectedItem = null;
            BorrowDate.SelectedDate = null;
            BorrowHour.Clear();
            BorrowMin.Clear();
            ReturnDate.SelectedDate = null;
            ReturnHour.Clear();
            ReturnMin.Clear();
            CompletionDate.SelectedDate= null;
            editConditiontext.Clear();
            editPaymenttext.Clear();
            editDescriptiontext.Clear();
            editPaymentStatustext.SelectedItem = null;
            CompletionError.Text = null;
            StatusError.Text = string.Empty;
            BorrowError.Text = string.Empty;
        }
        public virtual void OnDataSaved(EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }

        //Buttons
        //Save Buttons
        private void SaveBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to save these changes?", "Confirm Return", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string paymentText = editPaymenttext.Text;

                if (decimal.TryParse(paymentText, out decimal paymentAmount))
                {
                    ComboBoxItem selectedItemStatus = editStatustext.SelectedItem as ComboBoxItem;
                    ComboBoxItem selectedItemPaymentStatus = editPaymentStatustext.SelectedItem as ComboBoxItem;

                    if (chkVM.SaveEdit(editCheckoutIDtxt.Text, CompletionDate.SelectedDate.GetValueOrDefault(), selectedItemStatus.Content.ToString(),
                        editConditiontext.Text, editDescriptiontext.Text, paymentAmount, selectedItemPaymentStatus.Content.ToString()))
                    {
                        editStatustext.SelectedItem = null;
                        editPaymentStatustext.SelectedItem = null;
                        CompletionDate.SelectedDate = null;
                        BorrowDate.SelectedDate = null;
                        BorrowHour.Clear();
                        BorrowMin.Clear();
                        ReturnDate.SelectedDate = null;
                        ReturnHour.Clear();
                        ReturnMin.Clear();
                        editConditiontext.Clear();
                        editPaymenttext.Clear();
                        editDescriptiontext.Clear();


                        EditDetails.Visibility = Visibility.Collapsed;
                        Details.Visibility = Visibility.Visible;
                        TextClear();
                        LoadUser(Convert.ToInt32(editCheckoutIDtxt.Text));
                        OnDataSaved(EventArgs.Empty);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid payment amount.");
                }

                
            }
        }

        private void SaveBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4BBFFF"));
        }

        private void SaveBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA9FF"));
        }
        //Return Button
        private void ReturnBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EditDetails.Visibility = Visibility.Visible;
            Details.Visibility = Visibility.Collapsed;
        }

        private void ReturnBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ReturnBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1EE81E"));
        }

        private void ReturnBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ReturnBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF01D101"));
        }
        //Cancel Button
        private void CancelBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel return?", "Cancel Return", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                EditDetails.Visibility = Visibility.Collapsed;
                Details.Visibility = Visibility.Visible;
                TextClear();
                int checkoutID;

                if (int.TryParse(editCheckoutIDtxt.Text, out checkoutID))
                {
                    LoadUser(checkoutID);
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        // Text Changes
        private void editPaymenttext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editPaymenttext.Text) && editPaymenttext.Text.Length > 0)
            {
                editPaymenttxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editPaymenttxt.Visibility = Visibility.Visible;
            }
        }

        private void editConditiontext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editConditiontext.Text) && editConditiontext.Text.Length > 0)
            {
                editConditiontxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editConditiontxt.Visibility = Visibility.Visible;
            }
        }

        private void editDescriptiontext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editDescriptiontext.Text) && editDescriptiontext.Text.Length > 0)
            {
                editDescriptiontxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editDescriptiontxt.Visibility = Visibility.Visible;
            }
        }

        private void editPaymentStatustext_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editPaymentStatustext.SelectedItem != null)
            {
                ComboBoxItem selectedItem = editPaymentStatustext.SelectedItem as ComboBoxItem;
                editPaymentStatustext.Text = selectedItem.Content.ToString();
            }
            else
            {
                editPaymentStatustext.SelectedItem = 0;
                editPaymentStatustext.Text = "Select borrowing time window";
            }
        }

        //Borrow Date and Time
        private void BorrowHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BorrowHour.Text) && BorrowHour.Text.Length > 0)
            {
                BorrowHourtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                BorrowHourtxt.Visibility = Visibility.Visible;

            }
        }

        private void BorrowMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(BorrowMin.Text) && BorrowMin.Text.Length > 0)
            {
                BorrowMintxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                BorrowMintxt.Visibility = Visibility.Visible;
            }
        }
        private void BorrowAmPm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BorrowAmPm.SelectedItem == null)
            {
                ComboBoxItem selectedItem = BorrowAmPm.SelectedItem as ComboBoxItem;
                BorrowAmPm.Text = selectedItem.Content.ToString();
            }
            else
            {
                BorrowAmPm.SelectedItem = -1;
                BorrowAmPm.Text = "Select borrowing time window";
            }
        }
        //Return Date and Time
        private void ReturnHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ReturnHour.Text) && ReturnHour.Text.Length > 0)
            {
                ReturnHourtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReturnHourtxt.Visibility = Visibility.Visible;
            }
        }
        private void ReturnMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ReturnMin.Text) && ReturnMin.Text.Length > 0)
            {
                ReturnMintxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReturnMintxt.Visibility = Visibility.Visible;
            }
        }
        private void ReturnAmPm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReturnAmPm.SelectedItem == null)
            {
                ComboBoxItem selectedItem = ReturnAmPm.SelectedItem as ComboBoxItem;
                ReturnAmPm.Text = selectedItem.Content.ToString();
            }
            else
            {
                ReturnAmPm.SelectedItem = -1;
                ReturnAmPm.Text = "Select returning time window";
            }
        }


        private void editStatustext_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editStatustext.SelectedItem == null)
            {
                ComboBoxItem selectedItem = editStatustext.SelectedItem as ComboBoxItem;
            }
            else
            {
                editStatustext.SelectedItem = -1;
                editStatustext.Text = "Select reservation status";
            }
        }
    }
}
