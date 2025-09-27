using EQMS.Authentication.Register;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace EQMS.Details.EmployeeDetail
{
    /// <summary>
    /// Interaction logic for EmployeeDetails.xaml
    /// </summary>
    public partial class EmployeeDetails : Page
    {
        EmployeeDetailVM empVM = new EmployeeDetailVM();
        DetailWindow DetailWindow = new DetailWindow();
        public event EventHandler DataSaved;

        public EmployeeDetails()
        {
            InitializeComponent();
        }

        public EmployeeDetails(int id)
        {
            InitializeComponent();
            LoadUser(id);
        }

        public void LoadUser(int id)
        {
            empVM.LoadEmployeeByID(id);
            editIDtxt.Text = empVM.ID.ToString();
            editFNtext.Text = empVM.FirstName;
            editLNtext.Text = empVM.LastName;
            editEmailtext.Text = empVM.Email;
            editPhonetext.Text = empVM.Phone;
            editPositiontext.Text = empVM.Position;
            IDtxt.Text = empVM.ID.ToString();
            FNtxt.Text = empVM.FirstName;
            LNtxt.Text = empVM.LastName;
            Emailtxt.Text = empVM.Email;
            Phonetxt.Text = empVM.Phone;
            Positiontxt.Text = empVM.Position;
            if(empVM.SetImage(Convert.ToInt32(empVM.ID.ToString())) != null)
            {
                EmployeeImg.Source = empVM.SetImage(Convert.ToInt32(empVM.ID.ToString()));
                EditEmployeeImg.Source = empVM.SetImage(Convert.ToInt32(empVM.ID.ToString()));
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
        // Edit Position
        private void editPositiontext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editPositiontext.Text) && editPositiontext.Text.Length > 0)
            {
                editPositiontxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editPositiontxt.Visibility = Visibility.Visible;
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
                int employeeID;

                if (int.TryParse(editIDtxt.Text, out employeeID))
                {
                    LoadUser(employeeID);
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
            editPositiontext.Clear();
            FNError.Text = string.Empty;
            LNError.Text = string.Empty;
            EmailError.Text = string.Empty;
            PhoneError.Text = string.Empty;
            PositionError.Text = string.Empty;
        }
        //Save Edit Button
        private void SaveBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save these changes?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if(empVM.SaveEdit(editIDtxt.Text, editFNtext.Text, editLNtext.Text, editEmailtext.Text, editPhonetext.Text, editPositiontext.Text))
                {
                    EditDetails.Visibility = Visibility.Collapsed;
                    Details.Visibility = Visibility.Visible;
                    TextClear();
                    LoadUser(Convert.ToInt32(editIDtxt.Text));
                    OnDataSaved(EventArgs.Empty);
                }
                else
                {
                    FNError.Text = empVM.FNErrorText(editFNtext.Text);
                    LNError.Text = empVM.LNErrorText(editLNtext.Text);
                    EmailError.Text = empVM.EmailErrorText(editIDtxt.Text, editEmailtext.Text);
                    PhoneError.Text = empVM.PhoneErrorText(editIDtxt.Text, editPhonetext.Text);
                    PositionError.Text = empVM.PositionErrorText(editPositiontext.Text); ;
                }
            }
        }
        //Delete User
        private void DeleteBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to permanently remove this Employee?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (empVM.DeleteEmployee(editIDtxt.Text))
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
            empVM.BrowseImage(Convert.ToInt32(IDtxt.Text));
            EmployeeImg.Source = empVM.SetImage(Convert.ToInt32(IDtxt.Text));
            EditEmployeeImg.Source = empVM.SetImage(Convert.ToInt32(IDtxt.Text));
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
