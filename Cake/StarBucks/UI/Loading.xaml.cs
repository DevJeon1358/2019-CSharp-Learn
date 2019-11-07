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
        login loginWindow = new login();

        public Loading()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(delegate {
                Thread.Sleep(3000);
                OpenLoginWindow();
            });
        }

        private void OpenLoginWindow()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                loginWindow.Show();
                this.Close();
            }));
        }
    }
}
