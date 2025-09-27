using EQMS.Authentication;
using EQMS.Report;
using EQMS.Home;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EQMS.Reservation;

namespace EQMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            //Setting Home Page as default page
            Main.Navigate(new Uri("Home/Home.xaml", UriKind.Relative));
        }

        //Window Drag
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
        //Exit Window
        private void btnExit_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitBlack.png");
        }
        private void btnExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Application.Current.Shutdown();
            }
        }
        private void btnExit_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitRed.png");
        }
        //Maximize Window
        private void btnMax_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
                TaskBar.Margin = new Thickness(0, 10, 20, 0);
                mainPnl.Margin = new Thickness(0, 40, 0, 0);
            }
            else
            {
                this.WindowState = WindowState.Normal;
                this.Width = 1200;
                this.Height = 700;
                TaskBar.Margin = new Thickness(0, 0, 10, 0);
                mainPnl.Margin = new Thickness(0, 30, 0, 0);
            }
        }
        private void btnMax_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/maximizeBlack.png");
        }
        private void btnMax_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/maximizeRed.png");
        }
        //Minimize Window
        private void btnMin_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimizeBlack.png");
        }
        private void btnMin_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimizeRed.png");
        }
        private void btnMin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                WindowState = WindowState.Minimized;
            }
        }

        // ================= Navigation Bar =========================
        // Home
        private void homeBtn_MouseEnter_1(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)home, "/Resources/Img/homeWhite.png");
            homeCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtHome.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtHome.FontWeight = FontWeights.ExtraBold;
        }
        private void homeBtn_MouseLeave_1(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)home, "/Resources/Img/homeBlue.png");
            homeCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtHome.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtHome.FontWeight = FontWeights.Bold;
        }
        private void homeBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Home.Home();
        }
        //Reservation
        private void reservationBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)reservation, "/Resources/Img/reservationWhite.png");
            reservationCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtReservation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtReservation.FontWeight = FontWeights.ExtraBold;
        }
        private void reservationBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)reservation, "/Resources/Img/reservationBlue.png");
            reservationCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtReservation.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtReservation.FontWeight = FontWeights.Bold;
        }
        private void reservationBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Reservation.Reservation();
        }
        //Checkout
        private void checkoutBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)checkout, "/Resources/Img/checkoutWhite.png");
            checkoutCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtCheckout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtCheckout.FontWeight = FontWeights.ExtraBold;
        }
        private void checkoutBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)checkout, "/Resources/Img/checkoutBlue.png");
            checkoutCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtCheckout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtCheckout.FontWeight = FontWeights.Bold;
        }
        private void checkoutBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Checkout.Checkout();
        }
        //Equipment
        private void equipmentBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)equipment, "/Resources/Img/equipmentWhite.png");
            equipmentCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtEquipment.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtEquipment.FontWeight = FontWeights.ExtraBold;
        }
        private void equipmentBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)equipment, "/Resources/Img/equipmentBlue.png");
            equipmentCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtEquipment.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtEquipment.FontWeight = FontWeights.Bold;
        }
        private void equipmentBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Equipment.Equipment();
        }
        //Borrower
        private void borrowerBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)borrower, "/Resources/Img/borrowerWhite.png");
            borrowerCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtBorrower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtBorrower.FontWeight = FontWeights.ExtraBold;
        }
        private void borrowerBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)borrower, "/Resources/Img/borrowerBlue.png");
            borrowerCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtBorrower.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtBorrower.FontWeight = FontWeights.Bold;
        }
        private void borrowerBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Borrower.Borrower();
        }
        //Employee
        private void employeeBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)employee, "/Resources/Img/employeeWhite.png");
            employeeCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtEmployee.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtEmployee.FontWeight = FontWeights.ExtraBold;
        }
        private void employeeBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)employee, "/Resources/Img/employeeBlue.png");
            employeeCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtEmployee.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtEmployee.FontWeight = FontWeights.Bold;
        }
        private void employeeBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Employee.Employee();
        }

        //Report
        private void reportBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)report, "/Resources/Img/reportWhite.png");
            reportCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374957"));
            txtReport.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF212D36"));
            txtReport.FontWeight = FontWeights.ExtraBold;
        }
        private void reportBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)report, "/Resources/Img/reportBlue.png");
            reportCircle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE9EBF4"));
            txtReport.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtReport.FontWeight = FontWeights.Bold;
        }
        private void reportBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Main.Content = new Report.Report();
        }
        //Logout
        private void logoutBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)logout, "/Resources/Img/logoutRed.png");
            txtLogout.Foreground = Brushes.Red;
            txtLogout.FontWeight = FontWeights.ExtraBold;

        }
        private void logoutBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)logout, "/Resources/Img/logoutBlue.png");
            txtLogout.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF374957"));
            txtLogout.FontWeight = FontWeights.Bold;
        }
        private void logoutBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoginReg login = new LoginReg();
            login.Show();
            this.Close();
        }

        //Menu
        private void menu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            navBar.Width = 150;
            returnMenu.Visibility = Visibility.Visible;
            menu.Visibility = Visibility.Collapsed;
        }
        private void returnMenu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            navBar.Width = 60;
            returnMenu.Visibility = Visibility.Collapsed;
            menu.Visibility = Visibility.Visible;
        }

    }
}
