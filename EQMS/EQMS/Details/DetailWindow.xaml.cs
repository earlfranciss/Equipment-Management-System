using EQMS.Details.EmployeeDetail;
using EQMS.Borrower;
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
using System.Windows.Shapes;
using EQMS.Details.BorrowerDetail;
using EQMS.Details.EquipmentDetail;
using EQMS.Details.CheckoutDetail;
using EQMS.Details.ReservationDetail;

namespace EQMS.Details
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public event EventHandler DataSaved;
        public string id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

        public DetailWindow()
        {
            InitializeComponent();
        }

        // Exit Button
        private void btnExit_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitRed.png");
        }
        private void btnExit_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitBlack.png");
        }
        private void btnExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                OnDataSaved(EventArgs.Empty);
                this.Close();
            }
        }
        // Minimize Button
        private void btnMin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                WindowState = WindowState.Minimized;
            }
        }
        private void btnMin_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimizeBlack.png");
        }
        private void btnMin_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimizeRed.png");
        }
        // Window drag
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        //Change Image Source
        private void ChangeImageSource(Image image, string imagePath)
        {
            BitmapImage newImage = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            image.Source = newImage;
        }

        //Reload Table Contents
        public virtual void OnDataSaved(EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }

        private void Details_DataSaved(object sender, EventArgs e)
        {
            DataSaved?.Invoke(this, e);
        }

        public DetailWindow(EQMS.Employee.Employee emp, int id)
        {
            InitializeComponent();
            this.Width = 450;
            Main.Navigate(new Uri("Details/EmployeeDetail/EmployeeDetails.xaml", UriKind.Relative));
            EmployeeDetails empDetails = new EmployeeDetails(id);
            empDetails.DataSaved += Details_DataSaved;
            Main.Content = empDetails;
        }


        public DetailWindow(EQMS.Borrower.Borrower brwr, int id)
        {
            InitializeComponent();
            this.Width = 450;
            Main.Navigate(new Uri("Details/BorrowerDetail/BorrowerDetails.xaml", UriKind.Relative));
            BorrowerDetails brwrDetails = new BorrowerDetails(id);
            brwrDetails.DataSaved += Details_DataSaved;
            Main.Content = brwrDetails;
        }

        public DetailWindow(EQMS.Equipment.Equipment eqp, int id)
        {
            InitializeComponent();
            this.Width = 450;
            Main.Navigate(new Uri("Details/EquipmentDetail/EquipmentDetails.xaml", UriKind.Relative));
            EquipmentDetails eqpDetails = new EquipmentDetails(id);
            eqpDetails.DataSaved += Details_DataSaved;
            Main.Content = eqpDetails;
        }
        public DetailWindow(EQMS.Reservation.Reservation res, int id)
        {
            InitializeComponent();
            this.Width = 900;
            Main.Navigate(new Uri("Details/ReservationDetail/ReservationDetails.xaml", UriKind.Relative));
            ReservationDetails resDetails = new ReservationDetails(id);
            resDetails.DataSaved += Details_DataSaved;
            resDetails.Details.Visibility = Visibility.Visible;
            resDetails.AddDetails.Visibility = Visibility.Collapsed;
            Main.Content = resDetails;
        }
        public DetailWindow(EQMS.Checkout.Checkout chk, int id)
        {
            InitializeComponent();
            this.Width = 900;
            Main.Navigate(new Uri("Details/CheckoutDetail/CheckoutDetails.xaml", UriKind.Relative));
            CheckoutDetails chkDetails = new CheckoutDetails(id);
            chkDetails.DataSaved += Details_DataSaved;
            Main.Content = chkDetails;
        }

        public DetailWindow(bool addReservation)
        {
            InitializeComponent();
            this.Width = 900;
            Main.Navigate(new Uri("Details/ReservationDetail/ReservationDetails.xaml", UriKind.Relative));
            ReservationDetails resDetails = new ReservationDetails(addReservation);
            resDetails.Details.Visibility = Visibility.Collapsed;
            resDetails.AddDetails.Visibility = Visibility.Visible;
            resDetails.DataSaved += Details_DataSaved;
            Main.Content = resDetails;
        }
    }
}
