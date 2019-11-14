using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace StarBucks.UI
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window
    {
        public Loading()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(delegate {
                App.SeatData.Load(); //seatData로드
                OpenLoginWindow();
            });
        }

        private void OpenLoginWindow()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                var loginWindow = new login();
                App.login = loginWindow;

                loginWindow.Show();
                this.Close();
            }));
        }
    }
}
