using EQMS.Borrower;
using EQMS.Reservation;
using System;
using System.Collections.Generic;
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
using System.Xml.Linq;

namespace EQMS.Details.ReservationDetail
{
    /// <summary>
    /// Interaction logic for ReservationDetails.xaml
    /// </summary>
    public partial class ReservationDetails : Page
    {
        ReservationDetailVM resVM = new ReservationDetailVM();
        DetailWindow DetailWindow = new DetailWindow();
        public event EventHandler DataSaved;

        public ReservationDetails()
        {
            InitializeComponent();
        }
        public ReservationDetails(int id)
        {
            InitializeComponent();
            LoadUser(id);
        }

        public ReservationDetails(bool addReservation)
        {
            InitializeComponent();
            EditDetails.Visibility = Visibility.Collapsed;
            Details.Visibility = Visibility.Collapsed;
        }

        public void LoadUser(int id)
        {
            resVM.LoadReservationByID(id);
            ReservationIDtxt.Text = id.ToString();
            Statustxt.Text = resVM.Status;
            ReservationDatetxt.Text = resVM.ReservationDate.Date.ToString("yyyy-MM-dd");
            
            EquipmentIDtxt.Text = resVM.EquipmentID.ToString();
            EquipmentNametxt.Text = resVM.EquipmentName;
            EquipmentTypetxt.Text = resVM.Type;
            BorrowerIDtxt.Text = resVM.BorrowerID.ToString();
            BorrowerNametxt.Text = resVM.BorrowerLN + ", " + resVM.BorrowerFN;
            BorrowerRoletxt.Text = resVM.Role;
            Purposetxt.Text = resVM.Purpose;
            Subjecttxt.Text = resVM.Subject;
            Professortxt.Text = resVM.Professor;
            editReservationIDtxt.Text = id.ToString();
            editStatustext.SelectedItem = resVM.Status;
            reservationDate.SelectedDate = resVM.ReservationDate;
            BorrowDate.SelectedDate = resVM.BorrowDate.Date;
            BorrowHour.Text = (resVM.BorrowTime.Hours > 12) ? (resVM.BorrowTime.Hours - 12).ToString() : (resVM.BorrowTime.Hours).ToString();
            BorrowMin.Text = (resVM.BorrowTime.Minutes > 1) ? (resVM.BorrowTime.Minutes).ToString() : "00";
            BorrowAmPm.SelectedIndex = (resVM.BorrowTime.Hours >= 12 && resVM.BorrowTime.Minutes > 0) ? 1 : 0;
            ReturnDate.SelectedDate = resVM.ReturnDate.Date;
            ReturnHour.Text = (resVM.ReturnTime.Hours > 12) ? (resVM.ReturnTime.Hours - 12).ToString() : (resVM.ReturnTime.Hours).ToString();
            ReturnMin.Text = (resVM.ReturnTime.Minutes > 1) ? (resVM.ReturnTime.Minutes).ToString() : "00";
            ReturnAmPm.SelectedIndex = (resVM.ReturnTime.Hours >= 12 && resVM.ReturnTime.Minutes > 0) ? 1 : 0;
            editEquipmentIDtxt.Text = resVM.EquipmentID.ToString();
            editEquipmentNametext.Text = resVM.EquipmentName;
            editEquipmentTypetxt.Text = resVM.Type;
            editBorrowerIDtxt.Text = resVM.BorrowerID.ToString();
            editBorrowerNametxt.Text = resVM.BorrowerLN + ", " + resVM.BorrowerFN;
            editBorrowerRoletxt.Text = resVM.Role;
            editPurposetext.Text = resVM.Purpose;
            editSubjecttext.Text = resVM.Subject;
            editProfessortext.Text = resVM.Professor;
            string BorrowAMPM = (resVM.BorrowTime.Hours > 12) ? "pm" : "am";
            string ReturnAMPM = (resVM.ReturnTime.Hours > 12) ? "pm" : "am";
            BorrowDatetxt.Text = resVM.BorrowDate.Date.ToString("yyyy-MM-dd") + "  " + BorrowHour.Text + ":" + BorrowMin.Text + " " + BorrowAMPM;
            ReturnDatetxt.Text = resVM.ReturnDate.Date.ToString("yyyy-MM-dd") + "  " + ReturnHour.Text + ":" + ReturnMin.Text + " " + ReturnAMPM;
        }

        //Text Clear
        private void TextClear()
        {
            editStatustext.SelectedItem = null;
            reservationDate.SelectedDate = null;
            BorrowDate.SelectedDate = null;
            BorrowHour.Clear();
            BorrowMin.Clear();
            ReturnDate.SelectedDate = null;
            ReturnHour.Clear();
            ReturnMin.Clear();
            editEquipmentNametext.Clear();
            editPurposetext.Clear();
            editSubjecttext.Clear();
            editProfessortext.Clear();
            StatusError.Text = string.Empty;
            ReservationError.Text = string.Empty;
            BorrowError.Text = string.Empty;
            ReturnError.Text = string.Empty;
            EquipmentNameError.Text = string.Empty;
            PurposeError.Text = string.Empty;
            SubjectError.Text = string.Empty;
            ProfessorError.Text = string.Empty; 
        }

        public virtual void OnDataSaved(EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }



        // ----------------------------------------

        //Details Panel
        //Details Part Buttons
        private void EditBtn_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            EditDetails.Visibility = Visibility.Visible;
            Details.Visibility = Visibility.Collapsed;
        }
        //Cancel Reservation
        private void DeleteBtn_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to permanently cancel this Reservation?", "Confirm Cancelation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (resVM.CancelReservation(editReservationIDtxt.Text))
                {
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is DetailWindow)
                        {
                            OnDataSaved(EventArgs.Empty);
                            window.Close();
                            break;
                        }
                    }
                }
            }
        }
        private void DeleteBtn_MouseEnter_1(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = Brushes.LightGray;

        }
        private void DeleteBtn_MouseLeave_1(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = Brushes.Gray;

        }
        //Process Checkout
        private void CheckoutBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to proceed to Check-out?", "Process Check-out", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (resVM.ProcessCheckout(editReservationIDtxt.Text))
                {
                    OnDataSaved(EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Error occured upon processing Check-out.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CheckoutBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            CheckoutBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF31DF31"));
        }
        private void CheckoutBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            CheckoutBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF01D101"));
        }

        //Edit Panel
        private void CancelBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel these changes?", "Cancel Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                EditDetails.Visibility = Visibility.Collapsed;
                Details.Visibility = Visibility.Visible;
                TextClear();
                int reservationID;

                if (int.TryParse(editReservationIDtxt.Text, out reservationID))
                {
                    LoadUser(reservationID);
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SaveBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save these changes?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ComboBoxItem selectedItem = editStatustext.SelectedItem as ComboBoxItem;
                ComboBoxItem selectedItem1 = BorrowAmPm.SelectedItem as ComboBoxItem;
                ComboBoxItem selectedItem2 = ReturnAmPm.SelectedItem as ComboBoxItem;

                if (resVM.SaveEdit(editReservationIDtxt.Text, reservationDate.SelectedDate.GetValueOrDefault(), BorrowDate.SelectedDate.GetValueOrDefault(), BorrowHour.Text, BorrowMin.Text, selectedItem1.Content.ToString(), 
                    ReturnDate.SelectedDate.GetValueOrDefault(), ReturnHour.Text, ReturnMin.Text, selectedItem2.Content.ToString(), 
                    editEquipmentNametext.Text, editProfessortext.Text, editPurposetext.Text, selectedItem.Content.ToString(), editSubjecttext.Text))
                {
                    editStatustext.SelectedItem = null;
                    reservationDate.SelectedDate = null;
                    BorrowDate.SelectedDate = null;
                    BorrowHour.Clear();
                    BorrowMin.Clear();
                    ReturnDate.SelectedDate = null;
                    ReturnHour.Clear();
                    ReturnMin.Clear();
                    editEquipmentNametext.Clear();
                    editPurposetext.Clear();
                    editSubjecttext.Clear();
                    editProfessortext.Clear();


                    EditDetails.Visibility = Visibility.Collapsed;
                    Details.Visibility = Visibility.Visible;
                    TextClear();
                    LoadUser(Convert.ToInt32(editReservationIDtxt.Text));
                    OnDataSaved(EventArgs.Empty);
                }
                else
                {
                    StatusError.Text = resVM.StatusErrorText(editStatustext.SelectedItem.ToString());
                    ReservationError.Text = resVM.ReservationErrorText(reservationDate.SelectedDate.GetValueOrDefault(), BorrowDate.SelectedDate.GetValueOrDefault());
                    BorrowError.Text = resVM.BorrowDateErrorText(BorrowDate.SelectedDate.GetValueOrDefault()) + resVM.BorrowingHourMinErrorText(BorrowHour.Text, BorrowMin.Text);
                    ReturnError.Text = resVM.ReturnDateErrorText(ReturnDate.SelectedDate.GetValueOrDefault()) + resVM.ReturnHourMinErrorText(ReturnHour.Text, ReturnMin.Text);
                    EquipmentNameError.Text = resVM.EquipmentErrorText(editEquipmentNametext.Text);
                    PurposeError.Text = resVM.PurposeErrorText(editPurposetext.Text);
                    SubjectError.Text = resVM.SubjectErrorText(editSubjecttext.Text);
                    ProfessorError.Text = resVM.ProfessorErrorText(editProfessortext.Text);
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

        private void windowClose()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is DetailWindow)
                {
                    OnDataSaved(EventArgs.Empty);
                    window.Close();
                    break;
                }
            }
        }

        private void SetTime()
        {
            if (addReturnHour.Text.ToString().Equals(""))
            {
                addReturnHour.Text = "00";
            }
            if (addReturnMin.Text.ToString().Equals(""))
            {
                addReturnMin.Text = "00";
            }
            if (addBorrowHour.Text.ToString().Equals(""))
            {
                addBorrowHour.Text = "00";
            }
            if (addBorrowMin.Text.ToString().Equals(""))
            {
                addBorrowMin.Text = "00";
            }
        }


        private void CloseOtherWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is DetailWindow)
                {
                    window.Close();
                    break;
                }
            }

        }


        private void addReservationBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save these changes?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                SetTime();
                ComboBoxItem selectedItem = addEquipmentType.SelectedItem as ComboBoxItem;
                ComboBoxItem selectedItem1 = addBorrowAmPm.SelectedItem as ComboBoxItem;
                ComboBoxItem selectedItem2 = addReturnAmPm.SelectedItem as ComboBoxItem;
                ComboBoxItem selectedItem3 = addBorrowerRole.SelectedItem as ComboBoxItem;

                if (resVM.Reserve(addreservationDate.SelectedDate.GetValueOrDefault(), addBorrowDate.SelectedDate.GetValueOrDefault(), addBorrowHour.Text, addBorrowMin.Text,  selectedItem1.Content.ToString(),
                    addReturnDate.SelectedDate.GetValueOrDefault(), addReturnHour.Text, addReturnMin.Text, selectedItem2.Content.ToString(),
                    addEquipmentNametext.Text, selectedItem.Content.ToString(), addBorrowerIDtext.Text, addBorrowerFNtext.Text, addBorrowerLNtext.Text, 
                    selectedItem3.Content.ToString(), addBorrowerEmailtext.Text, addBorrowerPhonetext.Text, addProfessortext.Text, addPurposetext.Text, addSubjecttext.Text))
                {
                    addEquipmentType.SelectedItem = null;
                    addBorrowAmPm.SelectedItem = null;
                    addReturnAmPm.SelectedItem = null;
                    addBorrowerRole.SelectedItem = null;
                    addreservationDate.SelectedDate = null;
                    addBorrowDate.SelectedDate = null;
                    addReturnDate.SelectedDate = null;
                    addBorrowHour.Clear();
                    addBorrowMin.Clear();
                    addReturnHour.Clear();
                    addReturnMin.Clear();
                    addBorrowerEmailtext.Clear();
                    addBorrowerPhonetext.Clear();
                    addProfessortext.Clear();
                    addProfessortext.Clear();
                    addEquipmentNametext.Clear();
                    addBorrowerIDtext.Clear();
                    addBorrowerFNtext.Clear();
                    addBorrowerLNtext.Clear();


                    EditDetails.Visibility = Visibility.Collapsed;
                    Details.Visibility = Visibility.Visible;
                    AddDetails.Visibility = Visibility.Collapsed;
                    TextClear();
                    OnDataSaved(EventArgs.Empty);
                    CloseOtherWindow();
                }
                else
                {
                    addReservationError.Text = resVM.ReservationErrorText(addreservationDate.SelectedDate.GetValueOrDefault(), addBorrowDate.SelectedDate.GetValueOrDefault());
                    addBorrowError.Text = resVM.BorrowDateErrorText(addBorrowDate.SelectedDate.GetValueOrDefault()) + resVM.BorrowingHourMinErrorText(addBorrowHour.Text, addBorrowMin.Text);
                    addReturnError.Text = resVM.ReturnDateErrorText(addReturnDate.SelectedDate.GetValueOrDefault()) + resVM.ReturnHourMinErrorText(addReturnHour.Text, addReturnMin.Text);
                    addEquipmentNameError.Text = resVM.EquipmentErrorText(addEquipmentNametext.Text);
                    addEquipmentTypeError.Text = resVM.TypeErrorText(selectedItem.Content.ToString());
                    addBorrowerIDError.Text = resVM.IDErrorText(addBorrowerIDtext.Text);
                    addBorrowerFNError.Text = resVM.FNErrorText(addBorrowerFNtext.Text);
                    addBorrowerLNError.Text = resVM.LNErrorText(addBorrowerLNtext.Text);
                    addBorrowerRoleError.Text = resVM.RoleErrorText(selectedItem3.Content.ToString());
                    addBorrowerEmailError.Text = resVM.EmailErrorText(addBorrowerEmailtext.Text);
                    addBorrowerPhoneError.Text = resVM.PhoneErrorText(addBorrowerPhonetext.Text);
                    addSubjectError.Text = resVM.PurposeErrorText(addPurposetext.Text);
                    addSubjectError.Text = resVM.SubjectErrorText(addSubjecttext.Text);
                    addProfessorError.Text = resVM.ProfessorErrorText(addProfessortext.Text);
                }
            }
        }

        private void addReservationBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            addReservationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4BBFFF"));
        }

        private void addReservationBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            addReservationBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA9FF"));
        }

        private void addCancelBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to permanently cancel this Reservation?", "Confirm Cancelation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is DetailWindow)
                    {
                        OnDataSaved(EventArgs.Empty);
                        window.Close();
                        break;
                    }
                }
            }
        }


        //---------------------------------------------------------------
        //return reservationDate.SelectedDate.Value;

        public DateTime? SelectedDate
        {
            get { return reservationDate.SelectedDate; }
        }

        //Reservation Details
        private void editStatustext_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editStatustext.SelectedItem == null)
            {
                ComboBoxItem selectedItem = editStatustext.SelectedItem as ComboBoxItem;
            }
            else
            {
                editStatustext.SelectedItem = -1;
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
            }
            else
            {
                BorrowAmPm.SelectedItem = -1;
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
            }
            else
            {
                ReturnAmPm.SelectedItem = -1;
            }
        }
        private void editEquipmentNametext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editEquipmentNametext.Text) && editEquipmentNametext.Text.Length > 0)
            {
                editEquipmentNametxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editEquipmentNametxt.Visibility = Visibility.Visible;
            }
        }

        

        //Purpose and other Details
        private void editProfessortext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editProfessortext.Text) && editProfessortext.Text.Length > 0)
            {
                editProfessortxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editProfessortxt.Visibility = Visibility.Visible;
            }
        }
        private void editSubjecttext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editSubjecttext.Text) && editSubjecttext.Text.Length > 0)
            {
                editSubjecttxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editSubjecttxt.Visibility = Visibility.Visible;
            }
        }
        private void editPurposetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editPurposetext.Text) && editPurposetext.Text.Length > 0)
            {
                editPurposetxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editPurposetxt.Visibility = Visibility.Visible;
            }
        }


        //Text Changes
        private void addProfessortext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addProfessortext.Text) && addProfessortext.Text.Length > 0)
            {
                addProfessortxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addProfessortxt.Visibility = Visibility.Visible;
            }
        }

        private void addSubjecttext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addSubjecttext.Text) && addSubjecttext.Text.Length > 0)
            {
                addSubjecttxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addSubjecttxt.Visibility = Visibility.Visible;
            }
        }

        private void addPurposetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addPurposetext.Text) && addPurposetext.Text.Length > 0)
            {
                addPurposetxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addPurposetxt.Visibility = Visibility.Visible;
            }
        }

        private void addBorrowerPhonetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowerPhonetext.Text) && addBorrowerPhonetext.Text.Length > 0)
            {
                addBorrowerPhonetxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowerPhonetxt.Visibility = Visibility.Visible;
            }
        }

        private void addBorrowerEmailtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowerEmailtext.Text) && addBorrowerEmailtext.Text.Length > 0)
            {
                addBorrowerEmailtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowerEmailtxt.Visibility = Visibility.Visible;
            }
        }

        private void addBorrowerRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addBorrowerRole.SelectedItem == null)
            {
                ComboBoxItem selectedItem = addBorrowerRole.SelectedItem as ComboBoxItem;
            }
            else
            {
                addBorrowerRole.SelectedItem = 0;
            }
        }

        private void addBorrowerLNtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowerLNtext.Text) && addBorrowerLNtext.Text.Length > 0)
            {
                addBorrowerLNtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowerLNtxt.Visibility = Visibility.Visible;
            }
        }

        private void addBorrowerFNtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowerFNtext.Text) && addBorrowerFNtext.Text.Length > 0)
            {
                addBorrowerFNtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowerFNtxt.Visibility = Visibility.Visible;
            }
        }

        private void addBorrowerIDtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowerIDtext.Text) && addBorrowerIDtext.Text.Length > 0)
            {
                addBorrowerIDtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowerIDtxt.Visibility = Visibility.Visible;
            }
        }

        private void addEquipmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addEquipmentType.SelectedItem == null)
            {
                ComboBoxItem selectedItem = addEquipmentType.SelectedItem as ComboBoxItem;
            }
            else
            {
                addEquipmentType.SelectedItem = 0;
            }
        }

        private void addEquipmentNametext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addEquipmentNametext.Text) && addEquipmentNametext.Text.Length > 0)
            {
                addEquipmentNametxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addEquipmentNametxt.Visibility = Visibility.Visible;
            }
        }

        private void addReturnAmPm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addReturnAmPm.SelectedItem == null)
            {
                ComboBoxItem selectedItem = addReturnAmPm.SelectedItem as ComboBoxItem;

            }
            else
            {
                addReturnAmPm.SelectedItem = 0;

            }
        }

        private void addBorrowAmPm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addBorrowAmPm.SelectedItem == null)
            {
                ComboBoxItem selectedItem = addBorrowAmPm.SelectedItem as ComboBoxItem;
            }
            else
            {
                addBorrowAmPm.SelectedItem = 0;

            }
        }

        private void addBorrowHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowHour.Text) && addBorrowHour.Text.Length > 0)
            {
                addBorrowHourtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowHourtxt.Visibility = Visibility.Visible;
            }
        }

        private void addBorrowMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addBorrowMin.Text) && addBorrowMin.Text.Length > 0)
            {
                addBorrowMintxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addBorrowMintxt.Visibility = Visibility.Visible;
            }
        }

        private void addReturnMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addReturnMin.Text) && addReturnMin.Text.Length > 0)
            {
                addReturnMintxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addReturnMintxt.Visibility = Visibility.Visible;
            }
        }

        private void addReturnHour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addReturnHour.Text) && addReturnHour.Text.Length > 0)
            {
                addReturnHourtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                addReturnHourtxt.Visibility = Visibility.Visible;
            }
        }
    }
}
