using EQMS.Checkout;
using EQMS.Details;
using EQMS.Reservation;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace EQMS.Checkout
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Page
    {
        CheckoutViewModel viewModel = new CheckoutViewModel();

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


        public Checkout()
        {
            InitializeComponent();
            DataContext = viewModel;
            LoadData();
        }
        private void LoadData()
        {
            if (DataContext is CheckoutViewModel viewModel)
            {
                CheckoutDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
            }
        }

        private void CheckoutDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CloseOtherWindow();
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                if (row.Item is DataRowView)
                {
                    DataRowView dataRowView = row.Item as DataRowView;
                    DataRow row1 = dataRowView.Row;
                    Checkout selectedCheckout = ConvertDataRow(row1);

                    if (selectedCheckout != null)
                    {
                        int id = (selectedCheckout.ID);
                        DetailWindow detail = new DetailWindow(selectedCheckout, id);
                        detail.DataSaved += DetailWindow_DataSaved;
                        detail.Show();
                    }
                }
            }
        }

        // Refresh the grid table
        private void DetailWindow_DataSaved(object sender, EventArgs e)
        {
            LoadData();
        }


        private Checkout ConvertDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    Checkout checkout = new Checkout();
                    checkout.ID = Convert.ToInt32(row["ID"]);
                    return checkout;
                }
                else
                {
                    MessageBox.Show("DataRow is null.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
            {
                textSearch.Visibility = Visibility.Collapsed;

                if (DataContext is CheckoutViewModel viewModel)
                {
                    CheckoutDataGrid.ItemsSource = viewModel.SearchItem(txtSearch.Text).DefaultView;
                }
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;

                if (DataContext is CheckoutViewModel viewModel)
                {
                    CheckoutDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
                }
            }
        }
    }
}
