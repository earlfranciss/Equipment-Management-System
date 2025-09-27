using EQMS.Authentication.ChangePassword;
using EQMS.Authentication.Login;
using EQMS.Authentication.Register;
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

namespace EQMS.Authentication
{
    /// <summary>
    /// Interaction logic for LoginReg.xaml
    /// </summary>
    public partial class LoginReg : Window
    {
        
        LoginViewModel viewModel = new LoginViewModel();
        CPViewModel CPViewModel = new CPViewModel();
        RegViewModel RegViewModel = new RegViewModel();

        public LoginReg()
        {
            InitializeComponent();
        }
        
        // Window Drag
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

        // Exit Button
        private void Exit_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitRed.png");
        }
        private void ExitButton(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Application.Current.Shutdown();
            }
        }
        private void Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitWhite.png");
        }
        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/exitBlack.png");
        }

        // Minimize Button
        private void Minimize_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimizeRed.png");
        }
        private void MinimizeButton(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                WindowState = WindowState.Minimized;
            }
        }
        private void Minimize_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimizeWhite.png");
        }
        private void Minimize_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeImageSource((Image)sender, "/Resources/Img/minimize.png");
        }

        // Login Panel ---------------------------------------------------------------
        // Text Hint Changes
        private void IDNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIDNum.Text) && txtIDNum.Text.Length > 0)
            {
                textIDNum.Visibility = Visibility.Collapsed;
            }
            else
            {
                textIDNum.Visibility = Visibility.Visible;
            }
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }
        // Sign In Button
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Login(txtIDNum.Text, txtPassword.Password))
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
                LoginTxtClear();
            }
            else
            {
                IDNumError.Text = viewModel.UserErrorText(txtIDNum.Text);
                PasswordError.Text = viewModel.PassErrorText(txtIDNum.Text, txtPassword.Password);
            }
        }
        private void btnSignIn_MouseEnter(object sender, MouseEventArgs e)
        {
            brdSignIn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF27B0FC"));
            txtSignIn.Foreground = Brushes.White;

        }
        private void btnSignIn_MouseLeave(object sender, MouseEventArgs e)
        {
            brdSignIn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0FA9FF"));
            txtSignIn.Foreground = Brushes.Black;
        }
        // Forgot Password 
        private void textForgotPass_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoginPnl.Visibility = Visibility.Collapsed;
            RegisterPnl.Visibility = Visibility.Collapsed;
            FPPnl.Visibility = Visibility.Visible;
            LoginTxtClear();
        }
        private void textForgotPass_MouseEnter(object sender, MouseEventArgs e)
        {
            textForgotPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF27B0FC"));
        }
        private void textForgotPass_MouseLeave(object sender, MouseEventArgs e)
        {
            textForgotPass.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0F52FF"));
        }
        // Change Login to Register Panel
        private void txtSignup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoginPnl.Visibility = Visibility.Collapsed;
            RegisterPnl.Visibility = Visibility.Visible;
            LoginTxtClear();
        }
        private void txtSignup_MouseEnter(object sender, MouseEventArgs e)
        {
            txtSignup.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2964FD"));
        }
        private void txtSignup_MouseLeave(object sender, MouseEventArgs e)
        {
            txtSignup.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0F52FF"));
        }
        // Login Text Clear
        private void LoginTxtClear()
        {
            txtIDNum.Text = string.Empty;
            txtPassword.Password = string.Empty;
            IDNumError.Text = string.Empty;
            PasswordError.Text = string.Empty;
        }

        //Register Panel ----------------------------------------------------------------
        //Sign up button
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            if (RegViewModel.RegisterAcc(regFNtext.Text, regLNtext.Text, regEmailtext.Text, regPhonetext.Text, PassText()))
            {
                MessageBox.Show("Account Registration Successful! \n\nYour User ID is: " + RegViewModel.GetID(regFNtext.Text, regLNtext.Text), "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                LoginPnl.Visibility = Visibility.Visible;
                RegisterPnl.Visibility = Visibility.Collapsed;
                RegTxtClear();
            }
            else
            {
                RegFNError.Text = RegViewModel.FNErrorText(regFNtext.Text);
                RegLNError.Text = RegViewModel.LNErrorText(regLNtext.Text);
                RegEmailError.Text = RegViewModel.EmailErrorText(regEmailtext.Text);
                RegPhoneError.Text = RegViewModel.PhoneErrorText(regPhonetext.Text);
                RegPasswordError.Text = RegViewModel.PassErrorText(PassText());
            }


            
        }
        private string PassText()
        {
            if (txtregPassword.Visibility == Visibility.Visible)
            {
                return txtregPassword.Text;
            }
            else
            {
                return regPassword.Password;
            }
        }
        private void btnSignUp_MouseEnter(object sender, MouseEventArgs e)
        {
            brdSignUp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF27B0FC"));
            txtSignUp.Foreground = Brushes.White;
        }

        private void btnSignUp_MouseLeave(object sender, MouseEventArgs e)
        {
            brdSignUp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0FA9FF"));
            txtSignUp.Foreground = Brushes.Black;
        }

        //Show Password button
        private void showPassword_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            if (regPassword.PasswordChar == '\0')
            {
                regPassword.PasswordChar = '●';
                txtregPassword.Visibility = Visibility.Collapsed;
                regPassword.Visibility = Visibility.Visible;
                regPassword.Password = txtregPassword.Text;
                ChangeImageSource((Image)sender, "/Resources/Img/showPassword.png");
            }
            else
            {
                regPassword.PasswordChar = '\0';
                txtregPassword.Visibility= Visibility.Visible;
                regPassword.Visibility= Visibility.Collapsed;
                txtregPassword.Text = regPassword.Password;
                ChangeImageSource((Image)sender, "/Resources/Img/hidePassword.png");
            }
        }
        // Text Hint Change
        private void regPhonetext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(regPhonetext.Text) && regPhonetext.Text.Length > 0)
            {
                regPhonetxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                regPhonetxt.Visibility = Visibility.Visible;
            }
        }
        private void regEmailtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(regEmailtext.Text) && regEmailtext.Text.Length > 0)
            {
                regEmailtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                regEmailtxt.Visibility = Visibility.Visible;
            }
        }
        private void regLNtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(regLNtext.Text) && regLNtext.Text.Length > 0)
            {
                regLNtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                regLNtxt.Visibility = Visibility.Visible;
            }
        }
        private void regFNtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(regFNtext.Text) && regFNtext.Text.Length > 0)
            {
                regFNtxt.Visibility = Visibility.Collapsed;
            }
            else
            {
                regFNtxt.Visibility = Visibility.Visible;
            }
        }
        private void regPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if ((!string.IsNullOrEmpty(regPassword.Password) && regPassword.Password.Length > 0)
                || (!string.IsNullOrEmpty(txtregPassword.Text) && txtregPassword.Text.Length > 0))
            {
                txtRegPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtRegPassword.Visibility = Visibility.Visible;
            }
        }
        private void txtregPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((!string.IsNullOrEmpty(regPassword.Password) && regPassword.Password.Length > 0)
                || (!string.IsNullOrEmpty(txtregPassword.Text) && txtregPassword.Text.Length > 0))
            {
                txtRegPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtRegPassword.Visibility = Visibility.Visible;
            }
        }
        // Change Register to Login Panel
        private void txtSignin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoginPnl.Visibility = Visibility.Visible;
            RegisterPnl.Visibility = Visibility.Collapsed;
            RegTxtClear();
        }
        private void txtSignin_MouseEnter(object sender, MouseEventArgs e)
        {
            txtSignin.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2964FD"));
        }
        private void txtSignin_MouseLeave(object sender, MouseEventArgs e)
        {
            txtSignup.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0F52FF"));
        }
        // Register Text Clear
        private void RegTxtClear()
        {
            regFNtext.Text = string.Empty;
            regLNtext.Text = string.Empty;
            regEmailtext.Text = string.Empty;
            regPhonetext.Text = string.Empty;
            regPassword.Password = string.Empty;
            txtregPassword.Text = string.Empty;
            RegFNError.Text = string.Empty;
            RegLNError.Text = string.Empty;
            RegEmailError.Text = string.Empty;
            RegPhoneError.Text = string.Empty;
            RegPasswordError.Text = string.Empty;
        }

        // Forgot Password Panel ----------------------------------------------------------
        // Text Hint Change
        private void FPtxtIDNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FPtxtIDNum.Text) && FPtxtIDNum.Text.Length > 0)
            {
                FPtextIDNum.Visibility = Visibility.Collapsed;
            }
            else
            {
                FPtextIDNum.Visibility = Visibility.Visible;
            }
        }
        private void FPtxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FPtxtPassword.Password) && FPtxtPassword.Password.Length > 0)
            {
                FPtextPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                FPtextPassword.Visibility = Visibility.Visible;
            }
        }
        private void FPtxtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FPtxtConfirmPassword.Password) && FPtxtConfirmPassword.Password.Length > 0)
            {
                FPtextConfirmPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                FPtextConfirmPassword.Visibility = Visibility.Visible;
            }
        }
        // Change Password button
        private void FPbtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if(CPViewModel.ChangePassword(FPtxtIDNum.Text, FPtxtPassword.Password, FPtxtConfirmPassword.Password))
            {
                LoginPnl.Visibility = Visibility.Visible;
                FPPnl.Visibility = Visibility.Collapsed;
                FPTxtClear();
            }
            else
            {
                FPIDNumError.Text = CPViewModel.UserErrorText(FPtxtIDNum.Text);
                FPPasswordError.Text = CPViewModel.PassErrorText(FPtxtPassword.Password);
                FPConfirmPasswordError.Text = CPViewModel.ConfirmPassErrorText(FPtxtPassword.Password, FPtxtConfirmPassword.Password);
                FPTxtInputClear();
            }
        }
        // Signup change panel
        private void FPtxtSignup_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RegisterPnl.Visibility = Visibility.Visible;
            FPPnl.Visibility = Visibility.Collapsed;
            FPTxtClear();
        }
        // Forgot Pass Text Clear
        private void FPTxtClear()
        {
            FPtxtIDNum.Text = string.Empty;
            FPtxtPassword.Password = string.Empty;
            FPtxtConfirmPassword.Password = string.Empty;
            FPIDNumError.Text = string.Empty;
            FPPasswordError.Text = string.Empty;
            FPConfirmPasswordError.Text = string.Empty;
        }
        // Forgot Pass Text Clear
        private void FPTxtInputClear()
        {
            FPtxtPassword.Password = string.Empty;
            FPtxtConfirmPassword.Password = string.Empty;
        }

        private void FPbtnSignIn_MouseLeave(object sender, MouseEventArgs e)
        {
            FPbrdSignIn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0FA9FF"));
            FPtxtSignIn.Foreground = Brushes.Black;
        }

        private void FPbtnSignIn_MouseEnter(object sender, MouseEventArgs e)
        {
            FPbrdSignIn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF27B0FC"));
            FPtxtSignIn.Foreground = Brushes.White;
        }
        
    }
}

