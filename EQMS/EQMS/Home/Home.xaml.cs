using EQMS.Authentication.ChangePassword;
using EQMS;
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
using System.Data.SqlTypes;
using System.Data;
using EQMS.Details;
using EQMS.Checkout;

namespace EQMS.Home
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        HomeViewModel viewModel = new HomeViewModel();

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
        public TimeSpan TimeReturned { get; set; }

        public Home()
        {
            InitializeComponent();
            DataContext = viewModel;
            LoadData();
        }
        private void LoadData()
        {
            if (DataContext is HomeViewModel viewModel)
            {
                homeDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
            }
        }

        


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text) && txtSearch.Text.Length > 0)
            {
                textSearch.Visibility = Visibility.Collapsed;

                if (DataContext is HomeViewModel viewModel)
                {
                    homeDataGrid.ItemsSource = viewModel.SearchItem(txtSearch.Text).DefaultView;
                }
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;

                if (DataContext is HomeViewModel viewModel)
                {
                    homeDataGrid.ItemsSource = viewModel.GetDataFromDatabase().DefaultView;
                }
            }
        }

        private void homeDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {

            CloseOtherWindow();
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                if (row.Item is DataRowView)
                {
                    DataRowView dataRowView = row.Item as DataRowView;
                    DataRow row1 = dataRowView.Row;
                    if (row1["State"].Equals("Checked-out"))
                    {
                        EQMS.Checkout.Checkout selectedCheckout = ConvertCheckoutDataRow(row1);
                        if (selectedCheckout != null)
                        {
                            int id = (selectedCheckout.ID);
                            DetailWindow detail = new DetailWindow(selectedCheckout, id);
                            detail.DataSaved += DetailWindow_DataSaved;
                            detail.Show();
                        }
                    }
                    else
                    {
                        EQMS.Reservation.Reservation selectedReservation = ConvertReservationDataRow(row1);
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
        }

        private EQMS.Checkout.Checkout ConvertCheckoutDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    EQMS.Checkout.Checkout checkout = new EQMS.Checkout.Checkout();
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

        private EQMS.Reservation.Reservation ConvertReservationDataRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    EQMS.Reservation.Reservation reservation = new EQMS.Reservation.Reservation();
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

        //Change Image Source
        private void ChangeImageSource(Image image, string imagePath)
        {
            BitmapImage newImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            image.Source = newImage;
        }

        // Refresh the grid table
        private void DetailWindow_DataSaved(object sender, EventArgs e)
        {
            LoadData();
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
        private void Reservations_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow mainWindow)
                {
                    mainWindow.Main.Content = new Reservation.Reservation();
                    break;
                }
            }
        }

        private void Reservations_MouseEnter(object sender, MouseEventArgs e)
        {
            Reservations.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            ChangeImageSource((Image)reservationImg, "/Resources/Img/reservationHomeWhite.png");
            reservationTxt.Foreground = Brushes.White;
            BookedNum.Foreground = Brushes.White;
            Booked.Foreground = Brushes.White;
            ReservationOverdueNum.Foreground = Brushes.White;
            ReservationOverdue.Foreground = Brushes.White;
        }

        private void Reservations_MouseLeave(object sender, MouseEventArgs e)
        {
            Reservations.Background = Brushes.White;
            ChangeImageSource((Image)reservationImg, "/Resources/Img/reservationHomeBlue.png");
            reservationTxt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            BookedNum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            Booked.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D94A6"));
            ReservationOverdueNum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            ReservationOverdue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D94A6"));
        }

        private void Checkouts_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow mainWindow)
                {
                    mainWindow.Main.Content = new Checkout.Checkout();
                    break;
                }
            }
        }

        private void Checkouts_MouseEnter(object sender, MouseEventArgs e)
        {
            Checkouts.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            ChangeImageSource((Image)checkoutImg, "/Resources/Img/checkoutHomeWhite.png");
            checkoutTxt.Foreground = Brushes.White;
            OpenNum.Foreground = Brushes.White;
            Open.Foreground = Brushes.White;
            CheckoutOverdueNum.Foreground = Brushes.White;
            CheckoutOverdue.Foreground = Brushes.White;
        }

        private void Checkouts_MouseLeave(object sender, MouseEventArgs e)
        {
            Checkouts.Background = Brushes.White;
            ChangeImageSource((Image)checkoutImg, "/Resources/Img/checkoutHomeBlue.png");
            checkoutTxt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            OpenNum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            Open.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D94A6"));
            CheckoutOverdueNum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            CheckoutOverdue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D94A6"));
        }

        private void Maintenance_MouseEnter(object sender, MouseEventArgs e)
        {
            Maintenance.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            ChangeImageSource((Image)maintenanceImg, "/Resources/Img/maintenanceHomeWhite.png");
            maintenanceTxt.Foreground = Brushes.White;
            inProgressNum.Foreground = Brushes.White;
            inProgress.Foreground = Brushes.White;
            maintenanceBookedNum.Foreground = Brushes.White;
            maintenanceBooked.Foreground = Brushes.White;
        }

        private void Maintenance_MouseLeave(object sender, MouseEventArgs e)
        {
            Maintenance.Background = Brushes.White;
            ChangeImageSource((Image)maintenanceImg, "/Resources/Img/maintenanceBlue.png");
            maintenanceTxt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            inProgressNum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            inProgress.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D94A6"));
            maintenanceBookedNum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            maintenanceBooked.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7D94A6"));
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
