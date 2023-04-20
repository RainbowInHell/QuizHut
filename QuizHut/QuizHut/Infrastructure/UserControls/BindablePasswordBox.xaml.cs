namespace QuizHut.Infrastructure.UserControls
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class BindablePasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox));

        public static readonly DependencyProperty TagProperty =
        DependencyProperty.Register("Tag", typeof(object), typeof(BindablePasswordBox),
            new PropertyMetadata(null, TagPropertyChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public object Tag
        {
            get => GetValue(TagProperty);
            set => SetValue(TagProperty, value);
        }

        public BindablePasswordBox()
        {
            InitializeComponent();

            bindablePassword.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = bindablePassword.Password;
        }

        private static void TagPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.bindablePassword.Tag = e.NewValue;
            }
        }
    }
}