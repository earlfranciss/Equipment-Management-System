using EQMS.Details.EmployeeDetail;
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

namespace EQMS.Details.BorrowerDetail
{
    /// <summary>
    /// Interaction logic for BorrowerDetails.xaml
    /// </summary>
    public partial class BorrowerDetails : Page
    {
        public BorrowerDetails()
        {
            InitializeComponent();
        }

        BorrowerDetailVM brwrVM = new BorrowerDetailVM();
        DetailWindow DetailWindow = new DetailWindow();
        public event EventHandler DataSaved;

        public BorrowerDetails(int id)
        {
            InitializeComponent();
            LoadUser(id);
        }

        public void LoadUser(int id)
        {
            brwrVM.LoadBorrowerByID(id);
            editIDtxt.Text = brwrVM.ID.ToString();
            editFNtext.Text = brwrVM.FirstName;
            editLNtext.Text = brwrVM.LastName;
            editEmailtext.Text = brwrVM.Email;
            editPhonetext.Text = brwrVM.Phone;
            editRoletext.Text = brwrVM.Role;
            IDtxt.Text = brwrVM.ID.ToString();
            FNtxt.Text = brwrVM.FirstName;
            LNtxt.Text = brwrVM.LastName;
            Emailtxt.Text = brwrVM.Email;
            Phonetxt.Text = brwrVM.Phone;
            Roletxt.Text = brwrVM.Role;
            if (brwrVM.SetImage(Convert.ToInt32(brwrVM.ID.ToString())) != null)
            {
                BorrowerImg.Source = brwrVM.SetImage(Convert.ToInt32(brwrVM.ID.ToString()));
                EditBorrowerImg.Source = brwrVM.SetImage(Convert.ToInt32(brwrVM.ID.ToString()));
            }
        }


        //Edit Button Clicked
        private void EditBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EditDetails.Visibility = Visibility.Visible;
            Details.Visibility = Visibility.Collapsed;
        }

        //Edit First Name
        private void editFNtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editFNtext.Text) && editFNtext.Text.Length > 0)
            {
                editFNtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editFNtxt.Visibility = Visibility.Visible;
            }
        }
        //Edit Last Name
        private void editLNtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editLNtext.Text) && editLNtext.Text.Length > 0)
            {
                editLNtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editLNtxt.Visibility = Visibility.Visible;
            }
        }
        //Edit Email
        private void editEmailtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editEmailtext.Text) && editEmailtext.Text.Length > 0)
            {
                editEmailtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editEmailtxt.Visibility = Visibility.Visible;
            }
        }
        //Edit Phone Number
        private void editPhonetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editPhonetext.Text) && editPhonetext.Text.Length > 0)
            {
                editPhonetxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editPhonetxt.Visibility = Visibility.Visible;
            }
        }
        // Edit Role
        private void editRoletext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editRoletext.Text) && editRoletext.Text.Length > 0)
            {
                editRoletxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editRoletxt.Visibility = Visibility.Visible;
            }
        }

        // Cancel Edit Button
        private void Btn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel these changes?", "Cancel Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                EditDetails.Visibility = Visibility.Collapsed;
                Details.Visibility = Visibility.Visible;
                TextClear();
                int borrowerID;

                if (int.TryParse(editIDtxt.Text, out borrowerID))
                {
                    LoadUser(borrowerID);
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        //Text Clear
        private void TextClear()
        {
            editFNtext.Clear();
            editLNtext.Clear();
            editEmailtext.Clear();
            editPhonetext.Clear();
            editRoletext.Clear();
            FNError.Text = string.Empty;
            LNError.Text = string.Empty;
            EmailError.Text = string.Empty;
            PhoneError.Text = string.Empty;
            RoleError.Text = string.Empty;
        }
        //Save Edit Button
        private void SaveBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save these changes?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (brwrVM.SaveEdit(editIDtxt.Text, editFNtext.Text, editLNtext.Text, editEmailtext.Text, editPhonetext.Text, editRoletext.Text))
                {
                    EditDetails.Visibility = Visibility.Collapsed;
                    Details.Visibility = Visibility.Visible;
                    TextClear();
                    LoadUser(Convert.ToInt32(editIDtxt.Text));
                    OnDataSaved(EventArgs.Empty);
                }
                else
                {
                    FNError.Text = brwrVM.FNErrorText(editFNtext.Text);
                    LNError.Text = brwrVM.LNErrorText(editLNtext.Text);
                    EmailError.Text = brwrVM.EmailErrorText(editIDtxt.Text, editEmailtext.Text);
                    PhoneError.Text = brwrVM.PhoneErrorText(editIDtxt.Text, editPhonetext.Text);
                    RoleError.Text = brwrVM.RoleErrorText(editRoletext.Text); ;
                }
            }
        }
        //Delete User
        private void DeleteBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to permanently remove this Borrower?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (brwrVM.DeleteBorrower(editIDtxt.Text))
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

        public virtual void OnDataSaved(EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }

        private void SaveBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4BBFFF"));
        }

        private void SaveBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA9FF"));
        }

        private void DeleteBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF3F3F"));
        }

        private void DeleteBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            SaveBtn.Background = Brushes.Red;
        }

        private void SetImg_MouseUp(object sender, MouseButtonEventArgs e)
        {
            brwrVM.BrowseImage(Convert.ToInt32(IDtxt.Text));
            BorrowerImg.Source = brwrVM.SetImage(Convert.ToInt32(IDtxt.Text));
            EditBorrowerImg.Source = brwrVM.SetImage(Convert.ToInt32(IDtxt.Text));
        }

        private void SetImg_MouseEnter(object sender, MouseEventArgs e)
        {
            SetImg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4BBFFF"));
        }

        private void SetImg_MouseLeave(object sender, MouseEventArgs e)
        {
            SetImg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA9FF"));
        }

        private void EditSetImg_MouseEnter(object sender, MouseEventArgs e)
        {
            EditSetImg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4BBFFF"));
        }

        private void EditSetImg_MouseLeave(object sender, MouseEventArgs e)
        {
            EditSetImg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0FA9FF"));
        }
    }
}
