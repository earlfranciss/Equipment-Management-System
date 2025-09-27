using EQMS.Reservation;
using EQMS.Details;
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
using System.Data;
using System.ComponentModel;
using EQMS.Equipment;

namespace EQMS.Reservation
{
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservation : Page
    {
        ReservationViewModel viewModel = new ReservationViewModel();

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

        public Reservation()
        {
            InitializeComponent();
            DataContext = viewModel;
            LoadData();
        }
        private void LoadData()
        {
            if (DataContext is ReservationViewModel viewModel)
            {
                ReservationsDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
            }
        }

        private void ReservationsDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CloseOtherWindow();
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                if (row.Item is DataRowView)
                {
                    DataRowView dataRowView = row.Item as DataRowView;
                    DataRow row1 = dataRowView.Row;
                    Reservation selectedReservation = ConvertDataRow(row1);

                    if (selectedReservation != null)
                    {
                        int id = (selectedReservation.ID);
                        DetailWindow detail = new DetailWindow(selectedReservation, id);
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

        private Reservation ConvertDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    Reservation reservation = new Reservation();
                    reservation.ID = Convert.ToInt32(row["ID"]);
                    return reservation;
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

                if (DataContext is ReservationViewModel viewModel)
                {
                    ReservationsDataGrid.ItemsSource = viewModel.SearchItem(txtSearch.Text).DefaultView;
                }
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;

                if (DataContext is ReservationViewModel viewModel)
                {
                    ReservationsDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
                }
            }
        }

        private void AddReservation_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DetailWindow detail = new DetailWindow(true);
            detail.DataSaved += DetailWindow_DataSaved;
            detail.Show();
        }

        private void AddReservation_MouseEnter(object sender, MouseEventArgs e)
        {
            AddReservation.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            AddReservationtxt.Foreground = Brushes.White;
        }

        private void AddReservation_MouseLeave(object sender, MouseEventArgs e)
        {
            AddReservation.Background = Brushes.White;
            AddReservationtxt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
        }
    }
}
