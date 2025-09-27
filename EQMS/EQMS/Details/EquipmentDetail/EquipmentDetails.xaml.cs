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

namespace EQMS.Details.EquipmentDetail
{
    /// <summary>
    /// Interaction logic for EquipmentDetails.xaml
    /// </summary>
    public partial class EquipmentDetails : Page
    {
        EquipmentDetailVM eqpVM = new EquipmentDetailVM();
        DetailWindow DetailWindow = new DetailWindow();
        public event EventHandler DataSaved;

        public EquipmentDetails()
        {
            InitializeComponent();
        }


        public EquipmentDetails(int id)
        {
            InitializeComponent();
            LoadEquipment(id);
        }

        public void LoadEquipment(int id)
        {
            eqpVM.LoadEquipmentByID(id);
            editIDtxt.Text = eqpVM.ID.ToString();
            editNametext.Text = eqpVM.Name;
            editTypetext.Text = eqpVM.Type;
            editBrandtext.Text = eqpVM.Brand;
            editSerialtext.Text = eqpVM.Serial;
            IDtxt.Text = eqpVM.ID.ToString();
            Nametxt.Text = eqpVM.Name;
            Typetxt.Text = eqpVM.Type;
            Brandtxt.Text = eqpVM.Brand;
            Serialtxt.Text = eqpVM.Serial;
            if (eqpVM.SetImage(Convert.ToInt32(eqpVM.ID.ToString())) != null)
            {
                EquipmentImg.Source = eqpVM.SetImage(Convert.ToInt32(eqpVM.ID.ToString()));
                EditEquipmentImg.Source = eqpVM.SetImage(Convert.ToInt32(eqpVM.ID.ToString()));
            }
        }


        //Edit Button Clicked
        private void EditBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EditDetails.Visibility = Visibility.Visible;
            Details.Visibility = Visibility.Collapsed;
        }

        //Edit Name
        private void editNametext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editNametext.Text) && editNametext.Text.Length > 0)
            {
                editNametxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editNametxt.Visibility = Visibility.Visible;
            }
        }
        //Edit Type
        private void editTypetext_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(editTypetext.SelectedItem == null)
            {
                ComboBoxItem selectedItem = editTypetext.SelectedItem as ComboBoxItem;
                editTypetext.Text = selectedItem.Content.ToString(); 
            }
            else
            {
                editTypetext.SelectedItem = -1;
                editTypetext.Text = "Select equipment type";
            }
        }
        //Edit Brand
        private void editBrandtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editBrandtext.Text) && editBrandtext.Text.Length > 0)
            {
                editBrandtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editBrandtxt.Visibility = Visibility.Visible;
            }
        }
        //Edit Serial
        private void editSerialtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(editSerialtext.Text) && editSerialtext.Text.Length > 0)
            {
                editSerialtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                editSerialtxt.Visibility = Visibility.Visible;
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
                int equipmentID;

                if (int.TryParse(editIDtxt.Text, out equipmentID))
                {
                    LoadEquipment(equipmentID);
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
            editNametext.Clear();
            editTypetext.SelectedItem = -1;
            editTypetext.Text = "Enter equipment type";
            editBrandtext.Clear();
            editSerialtext.Clear();
            NameError.Text = string.Empty;
            TypeError.Text = string.Empty;
            BrandError.Text = string.Empty;
            SerialError.Text = string.Empty;
        }
        //Save Edit Button
        private void SaveBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to save these changes?", "Confirm Edit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            ComboBoxItem selectedItem = editTypetext.SelectedItem as ComboBoxItem;

            if (result == MessageBoxResult.Yes)
            {
                if (eqpVM.SaveEdit(editIDtxt.Text, editNametext.Text, selectedItem.Content.ToString(), editBrandtext.Text, editSerialtext.Text))
                {
                    EditDetails.Visibility = Visibility.Collapsed;
                    Details.Visibility = Visibility.Visible;
                    TextClear();
                    LoadEquipment(Convert.ToInt32(editIDtxt.Text));
                    OnDataSaved(EventArgs.Empty);
                }
                else
                {
                    NameError.Text = eqpVM.NameErrorText(editNametext.Text);
                    TypeError.Text = eqpVM.TypeErrorText(selectedItem.Content.ToString());
                    BrandError.Text = eqpVM.BrandErrorText(editBrandtext.Text);
                    SerialError.Text = eqpVM.SerialErrorText(editIDtxt.Text, editSerialtext.Text);
                }
            }
        }
        //Delete User
        private void DeleteBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to permanently remove this Equipment?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (eqpVM.DeleteEquipment(editIDtxt.Text))
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
            eqpVM.BrowseImage(Convert.ToInt32(IDtxt.Text));
            EquipmentImg.Source = eqpVM.SetImage(Convert.ToInt32(IDtxt.Text));
            EditEquipmentImg.Source = eqpVM.SetImage(Convert.ToInt32(IDtxt.Text));
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
