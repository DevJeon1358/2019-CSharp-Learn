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
        Main main = new Main();

        public Loading()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(delegate {
                Thread.Sleep(3000);
                App.SeatData.Load(); //seatData로드
                OpenMainWindow();
            });
        }

        private void OpenMainWindow()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                main.Show();
                this.Close();
            }));
        }
    }
}
