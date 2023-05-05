namespace QuizHut.Views.Windows
{
    using System;
    using System.Windows;
    using System.Runtime.InteropServices;
    using System.Windows.Interop;

    public partial class MainView : Window
    {
        public MainView(object dataContex)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            DataContext = dataContex;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void MVControlBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) 
        { 
            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            SendMessage(interopHelper.Handle, 161, 2, 0);
        }
    }
}