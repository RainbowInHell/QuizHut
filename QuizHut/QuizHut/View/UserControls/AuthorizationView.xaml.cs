namespace QuizHut.View.UserControls
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

        private void AVPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (AVPasswordBox.Password.Length > 0)
                AVShowHidePasswImg.Visibility = Visibility.Visible;
            else
                AVShowHidePasswImg.Visibility = Visibility.Hidden;
        }

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

    //Attached property for access to Length of PasswordBox
    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
}

