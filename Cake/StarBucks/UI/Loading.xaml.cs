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
            Thread.Sleep(3000);
            openMainWindow();
        }

        private void openMainWindow()
        {
            Main main = new Main();

            main.Show();
            this.Close();
        }
    }
}
