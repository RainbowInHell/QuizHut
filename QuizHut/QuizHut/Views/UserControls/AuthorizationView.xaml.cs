namespace QuizHut.Views.UserControls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Логика взаимодействия для AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : UserControl
    {
        public AuthorizationView()
        {
            InitializeComponent();
        }

        private void AVShowHidePasswImg_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void AVShowHidePasswImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void AVShowHidePasswImg_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        //private void AVPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (AVPasswordBox.Password.Length > 0)
        //        AVShowHidePasswImg.Visibility = Visibility.Visible;
        //    else
        //        AVShowHidePasswImg.Visibility = Visibility.Hidden;
        //}

        private void ShowPassword()
        {
            AVShowHidePasswImg.Source = new BitmapImage(new Uri("pack://application:,,,/QuizHut;component/Images/LoginViewImages/EyePic2.png"));
            AVVisiblePasswBox.Visibility = Visibility.Visible;
            AVPasswordBox.Visibility = Visibility.Hidden;
            AVVisiblePasswBox.Text = AVPasswordBox.Password;
        }

        private void HidePassword()
        {
            AVShowHidePasswImg.Source = new BitmapImage(new Uri("pack://application:,,,/QuizHut;component/Images/LoginViewImages/EyePic.png"));
            AVVisiblePasswBox.Visibility = Visibility.Hidden;
            AVPasswordBox.Visibility = Visibility.Visible;
            AVPasswordBox.Focus();
        }
    }
}

